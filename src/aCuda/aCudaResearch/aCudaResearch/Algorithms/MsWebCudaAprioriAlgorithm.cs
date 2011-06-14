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
        private Color one = Color.FromArgb(1);

        public override void Run(ExecutionSettings executionSettings, bool printRules)
        {
            builder = new MsDataBuilder();
            var data = builder.BuildInstance(executionSettings);
            var elementsList = data.Elements.Keys.ToList();
            var transactionsList = data.Transactions.Keys.ToList();
            var bitmapWrapper = PrepareBitmapWrapper(data, elementsList, transactionsList);
            var elementsFrequencies = CalculateElementsFrequencies(bitmapWrapper);
            var frequentSets = elementsList
                .Where(e => elementsFrequencies[elementsList.IndexOf(e)] >= executionSettings.MinSup * transactionsList.Count)
                .Select(element => new List<int> { element })
                .ToList();
            var frequentItemSets = frequentSets.ToDictionary(set => new FrequentItemSet<int>(set),
                                                             set => elementsFrequencies[elementsList.IndexOf(set[0])]);
            List<List<int>> candidates;

            while ((candidates = GenerateCandidates(frequentSets)).Count > 0)
            {
                // 1. tranlate into elements Id's
                foreach (var candidate in candidates)
                {
                    for (var i = 0; i < candidate.Count; i++)
                    {
                        candidate[i] = elementsList.IndexOf(candidate[i]);
                    }
                }

                // 2. execute CUDA counting
                candidates = GetFrequentSets(candidates, executionSettings.MinSup, bitmapWrapper, elementsList);

                // 3. translate back from elements Id's
                foreach (var candidate in candidates)
                {
                    for (int i = 0; i < candidate.Count; i++)
                    {
                        candidate[i] = elementsList[candidate[i]];
                    }
                }

                // leave only these sets which are frequent
                //candidates =
                //    candidates.Where(set => set.IsFrequent(elementsList, bitmapWrapper, transactionsList.Count, executionSettings.MinSup)).ToList();

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

        private List<List<int>> GetFrequentSets(IList<List<int>> candidates, double minSup, BitmapWrapper bitmapWrapper, IList<int> elementsList)
        {
            var output = CalculateElementsFrequencies<int>(candidates, bitmapWrapper);

            return new List<List<int>>();
        }


        private static int[] CalculateElementsFrequencies<T>(IList<List<int>> candidates, BitmapWrapper bitmapWrapper)
        {
            int frequencesSize = ProjectConstants.BlockSize * ProjectConstants.GridSize;
            var elementsFrequencies = new int[candidates.Count];

            using (var cuda = new CUDA(0, true))
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                        "apriori_count1.cubin");

                cuda.LoadModule(path);

                var inputData = cuda.CopyHostToDevice(bitmapWrapper.RgbValues);

                var inputSets = new int[candidates.Count * candidates[0].Count];
                var ind = 0;
                foreach (var candidate in candidates)
                {
                    foreach (var i in candidate)
                    {
                        inputSets[ind] = i;
                        ind++;
                    }
                }

                var inputSetData = cuda.CopyHostToDevice(inputSets.ToArray());
                var frequentMatrix = cuda.Allocate(new int[frequencesSize * candidates.Count]);
                var frequentTable = cuda.Allocate(new int[candidates.Count]);

                CallTheFrequencyMatrixCount(cuda, inputData, inputSetData, frequentMatrix, bitmapWrapper, candidates[0].Count, candidates.Count);
                CallTheFrequencyTableCount(cuda, frequentMatrix, frequentTable, frequencesSize, candidates.Count);

                var elementsFrequenciesMatrix = new int[frequencesSize * candidates.Count];
                cuda.CopyDeviceToHost(frequentMatrix, elementsFrequenciesMatrix);
                cuda.CopyDeviceToHost(frequentTable, elementsFrequencies);

                cuda.Free(inputData);
                cuda.Free(frequentMatrix);
                cuda.UnloadModule();
            }
            return elementsFrequencies;
        }

        private static void CallTheFrequencyMatrixCount(CUDA cuda, CUdeviceptr deviceInput, CUdeviceptr deviceInputSet, CUdeviceptr deviceOutput, BitmapWrapper wrapper, int setSize, int sets)
        {
            new CudaFunctionCall(cuda, Names.CountSetsFrequencyMatrix)
                .AddParameter(deviceInput)
                .AddParameter(deviceInputSet)
                .AddParameter(deviceOutput)
                .AddParameter((uint)wrapper.Width)
                .AddParameter((uint)wrapper.Height)
                .AddParameter((uint)setSize)
                .AddParameter((uint)sets)
                .Execute(ProjectConstants.BlockSize, ProjectConstants.BlockSize, 1, ProjectConstants.GridSize, ProjectConstants.GridSize);
        }

        private static void CallTheFrequencyTableCount(CUDA cuda, CUdeviceptr deviceInput, CUdeviceptr deviceOutput, int width, int height)
        {
            new CudaFunctionCall(cuda, Names.CountSetsFrequencyTable)
                .AddParameter(deviceInput)
                .AddParameter(deviceOutput)
                .AddParameter((uint)width)
                .AddParameter((uint)height)
                .Execute(ProjectConstants.BlockSize, ProjectConstants.BlockSize, 1, ProjectConstants.GridSize, ProjectConstants.GridSize);
        }

        private BitmapWrapper PrepareBitmapWrapper(MsInstance<int> data, List<int> elementsList, List<int> transactionsList)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var bitmap = BuildTransactionsBitmap(data, transactionsList, elementsList);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            var bitmapWrapper = BitmapWrapper.ConvertBitmap(bitmap);
            return bitmapWrapper;
        }

        private int[] CalculateElementsFrequencies(BitmapWrapper bitmapWrapper)
        {
            var elementsFrequencies = new int[bitmapWrapper.Width];
            using (var cuda = new CUDA(0, true))
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                        "apriori_count1.cubin");

                cuda.LoadModule(path);

                var inputData = cuda.CopyHostToDevice(bitmapWrapper.RgbValues);
                var answer = cuda.Allocate(new int[bitmapWrapper.Width]);
                CallTheFrequencyCount(cuda, inputData, answer, bitmapWrapper);
                cuda.CopyDeviceToHost(answer, elementsFrequencies);

                cuda.Free(inputData);
                cuda.Free(answer);
                cuda.UnloadModule();
            }
            return elementsFrequencies;
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
            new CudaFunctionCall(cuda, Names.CountFrequency)
                .AddParameter(deviceInput)
                .AddParameter(deviceOutput)
                .AddParameter((uint)wrapper.Width)
                .AddParameter((uint)wrapper.Height)
                .Execute(ProjectConstants.BlockSize, ProjectConstants.BlockSize, 1, ProjectConstants.GridSize, ProjectConstants.GridSize);
        }
    }
}
