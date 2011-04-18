using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aCudaResearch.Algorithms.FpGrowth;
using aCudaResearch.Data;
using aCudaResearch.Data.MsWeb;

namespace aCudaResearch.Algorithms
{
    public class MsWebFpGrowthAlgorithm : MsWebAbstractAlgorithm
    {
        public override void Run(ExecutionSettings executionSettings)
        {
            builder = new MsDataBuilder();
            var data = builder.BuildInstance(executionSettings);

            var compressedDatabase = FrequentSetGenerator.Generate(data.Database, executionSettings.MinSup);

            var tree = new FpTree<int>();
            tree.BuildFpTreeFromData(compressedDatabase);

            var treeExplorer = new TreeExplorer<int>((int)(data.Database.Keys.Count * executionSettings.MinSup));
            var rules = treeExplorer.GenerateRuleSet(tree, executionSettings.MinConf);

            var result = PrintRules(rules, executionSettings.DataSourcePath, executionSettings.MinSup, executionSettings.MinConf, data.Database.Keys.Count, data.Elements);
            Console.WriteLine(result);
        }

        private static string PrintRules(IList<DecisionRule<int>> rules, string inputFile, double minSupport, double minConfidence, int databaseSize, Dictionary<int, MsElement> elements)
        {
            var sb = new StringBuilder();
            sb.Append(inputFile + "\n");
            sb.Append(minSupport + "\n");
            sb.Append(minConfidence + "\n");
            sb.Append(rules.Count + "\n\n\n");
            sb.Append("** RULES\n\n");

            for (var i = 0; i < rules.Count; i++)
            {
                sb.Append((i + 1) + ": " + rules[i].ToString(databaseSize, elements) + "\n\n");
            }

            return sb.ToString();
        }
    }
}
