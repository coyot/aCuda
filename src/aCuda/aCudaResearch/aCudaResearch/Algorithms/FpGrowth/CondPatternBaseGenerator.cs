using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch.Algorithms.FpGrowth
{
    /// <summary>
    /// Warunkowa baza wzorca.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CondPatternBaseGenerator<T>
    {
        /// <summary>
        /// Generuje warunkową bazę wzorca na podstawie bazy wejściowej i minimalnego wsparcia.
        /// </summary>
        /// <param name="database">baza w postaci słownika zawierającego listy elementów wraz z ich wsparciem</param>
        /// <param name="minSup">minimalne wsparcie, jakie muszą posiadac elementy, aby znalazły się w warunkowej bazie wzorca</param>
        /// <returns>warunkowa baza wzorca w postaci słownika, którego kluczem są listy elementów, a wartościami ich wsparcia</returns>
        public Dictionary<T[], int> Generate(Dictionary<List<TreeNode<T>>, int> database, int minSup)
        {
            Dictionary<T, int> frequentSets = new Dictionary<T, int>();
            Dictionary<T[], int> result = new Dictionary<T[], int>();

            foreach (var key in database.Keys)
            {
                foreach (var node in key)
                {
                    if (frequentSets.ContainsKey(node.Value))
                        frequentSets[node.Value] += database[key];
                    else
                        frequentSets[node.Value] = database[key];
                }
            }

            foreach (var key in database.Keys)
            {
                List<T> elements = new List<T>();
                foreach (var node in key)
                {
                    if (frequentSets[node.Value] >= minSup)
                    {
                        elements.Add(node.Value);
                    }
                }
                if (elements.Count > 0)
                {
                    result[elements.ToArray()] = database[key];
                }
            }
            return result;
        }
    }
}
