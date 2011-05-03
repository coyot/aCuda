using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aCudaResearch.Data.MsWeb;
using aCudaResearch.Helpers;

namespace aCudaResearch.Algorithms
{
    public class MsWebAprioriAlgorithm : MsWebAbstractAlgorithm
    {
        public override void Run(ExecutionSettings executionSettings)
        {
            builder = new MsDataBuilder();
            var data = builder.BuildInstance(executionSettings);

            var frequentSets = data.Elements.Keys.Select(element => new List<int> { element }).ToList();

            frequentSets = frequentSets.Where(set => set.IsFrequent<int>(data.Transactions, executionSettings.MinSup)).ToList();
            List<List<int>> candidates;

            while ((candidates = GenerateCandidates(frequentSets)).Count > 0)
            {
                // leave only these sets which are frequent
                candidates =
                    candidates.Where(set => set.IsFrequent(data.Transactions, executionSettings.MinSup)).ToList();

                if (candidates.Count > 0)
                {
                    frequentSets = candidates;
                }
                else
                {
                    // we don't have any more candidates
                    break;
                }
            }

            //here we should do something with the candidates

            Console.WriteLine("test");
        }

        public List<List<int>> GenerateCandidates(List<List<int>> source)
        {
            var candidates = new List<List<int>>();
            for (var i = 0; i < source.Count; i++)
            {
                for (var j = i+1; j < source.Count; j++)
                {
                    if(source[i].Take(source[i].Count-1).AreEqual(source[j].Take(source[j].Count-1)))
                    {
                        candidates.Add((List<int>)source[i].Merge(source[j]));
                    }
                }
            }


            return candidates;
        } 
    }
}
