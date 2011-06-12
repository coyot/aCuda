using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using aCudaResearch.Cuda;
using aCudaResearch.Data;
using aCudaResearch.Data.MsWeb;
using aCudaResearch.Helpers;
using System.IO;
using System.Linq;
using System.Reflection;
using GASS.CUDA;
using GASS.CUDA.Types;

namespace aCudaResearch.Algorithms
{
    public class MsWebCudaAprioriAlgorithm : MsWebAbstractAlgorithm
    {
        private const int BlockSize = 1;
        private Color one = Color.FromArgb(1);

        public override void Run(ExecutionSettings executionSettings, bool printRules)
        {
            builder = new MsDataBuilder();
            var data = builder.BuildInstance(executionSettings);
            var elementsList = data.Elements.Keys.ToList();
            var transactionsList = data.Transactions.Keys.ToList();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var bitmap = BuildTransactionsBitmap(data, transactionsList, elementsList);
            stopwatch.Stop();

            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            var values = BitmapWrapper.ConvertBitmap(bitmap);

            var output = new int[values.Width];
            using (var cuda = new CUDA(0, true))
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                        "apriori_count1.cubin");

                cuda.LoadModule(path);

                var inputData = cuda.CopyHostToDevice(values.RgbValues);
                var answer = cuda.Allocate(new int[values.Width]);
                CallTheFrequencyCount(cuda, inputData, answer, values);
                cuda.CopyDeviceToHost(answer, output);

                cuda.Free(inputData);
                cuda.Free(answer);
                cuda.UnloadModule();
            }

            var frequentSets = elementsList.Where(e => output[elementsList.IndexOf(e)] >= executionSettings.MinSup * transactionsList.Count).Select(element => new List<int> { element }).ToList();

            var frequentItemSets = frequentSets.ToDictionary(set => new FrequentItemSet<int>(set),
                                                             set => output[elementsList.IndexOf(set[0])]);
            List<List<int>> candidates;

            while ((candidates = GenerateCandidates(frequentSets)).Count > 0)
            {
                //! sprawdź czy któryś podzbiór k-1 elementowy kadydatów nie jest w frequentSets => wywal go!

                // leave only these sets which are frequent
                candidates =
                    candidates.Where(set => set.IsFrequent(data.Transactions, executionSettings.MinSup)).ToList();

                if (candidates.Count > 0)
                {
                    frequentSets = candidates;
                    foreach (var candidate in candidates)
                    {
                        frequentItemSets.Add(new FrequentItemSet<int>(candidate), candidate.GetSupport(data.Transactions));
                    }
                }
                else
                {
                    // we don't have any more candidates
                    break;
                }
            }

            //here we should do something with the candidates
            var decisionRules = new List<DecisionRule<int>>();

            foreach (var frequentSet in frequentSets)
            {
                var subSets = EnumerableHelper.GetSubsets(frequentSet);

                foreach (var t in subSets)
                {
                    var leftSide = new FrequentItemSet<int>(t);
                    for (var j = 0; j < subSets.Count; j++)
                    {
                        var rightSide = new FrequentItemSet<int>(subSets[j]);
                        if (rightSide.ItemSet.Count != 1 || !FrequentItemSet<int>.SetsSeparated(rightSide, leftSide))
                        {
                            continue;
                        }

                        if (frequentItemSets.ContainsKey(leftSide))
                        {
                            var confidence = (double)frequentItemSets[new FrequentItemSet<int>(frequentSet)] / frequentItemSets[leftSide];
                            if (confidence >= executionSettings.MinConf)
                            {
                                var rule = new DecisionRule<int>(leftSide.ItemSet, rightSide.ItemSet, frequentItemSets[new FrequentItemSet<int>(frequentSet)], confidence);
                                decisionRules.Add(rule);
                            }
                        }
                    }
                }
            }

            // cuda tests here!!!
            
            if (!printRules) return;

            var result = PrintRules(decisionRules, executionSettings.DataSourcePath, executionSettings.MinSup, executionSettings.MinConf, data.Transactions.Keys.Count, data.Elements);
            Console.WriteLine(result);
        }

        private Bitmap BuildTransactionsBitmap(MsInstance<int> data, IList<int> transactionsList, IList<int> elementsList)
        {
            var bitmap = new Bitmap(elementsList.Count, transactionsList.Count);
            for (var i = 0; i < elementsList.Count; i++)
            {
                for (var j = 0; j < transactionsList.Count; j++)
                {
                    if (data.Transactions[transactionsList[j]].Contains(elementsList[i]))
                    {
                        bitmap.SetPixel(i, j, one);
                    }
                }
            }
            return bitmap;
        }

        public List<List<int>> GenerateCandidates(List<List<int>> source)
        {
            var candidates = new List<List<int>>();
            for (var i = 0; i < source.Count; i++)
            {
                for (var j = i + 1; j < source.Count; j++)
                {
                    if (source[i].Take(source[i].Count - 1).AreEqual(source[j].Take(source[j].Count - 1)))
                    {
                        candidates.Add((List<int>)source[i].Merge(source[j]));
                    }
                }
            }

            return candidates;
        }

        protected virtual void CallTheFrequencyCount(CUDA cuda, CUdeviceptr deviceInput, CUdeviceptr deviceOutput, BitmapWrapper wrapper)
        {
            new CudaFunctionCall(cuda, "count_frequency")
                .AddParameter(deviceInput)
                .AddParameter(deviceOutput)
                .AddParameter((uint)wrapper.Width)
                .AddParameter((uint)wrapper.Height)
                .Execute(BlockSize, BlockSize, 1, 5, 1);
        }
    }
}
