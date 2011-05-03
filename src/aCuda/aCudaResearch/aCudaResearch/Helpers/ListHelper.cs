using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch.Helpers
{
    public static class ListHelper
    {
        /// <summary>
        /// Checks if the list of elements is frequent for supported
        /// transactions and specified support.
        /// </summary>
        /// <typeparam name="T">Type of the elements ID's</typeparam>
        /// <param name="toCheck">List of values which should be checked</param>
        /// <param name="transactions">List of transactions</param>
        /// <param name="support">The minimal support for the frequency</param>
        /// <returns>True if the set is frequent, false otherwise</returns>
        public static bool IsFrequent<T>(this IList<T> toCheck, Dictionary<T, T[]> transactions, double support)
        {
            var transactionsList = transactions.Values.ToList();

            foreach (var element in toCheck)
            {
                var tmpTransactionsList =
                    transactionsList.Where(transaction => transaction.Contains(element)).ToList();

                transactionsList = tmpTransactionsList;
            }
            return transactionsList.Count >= (support*transactions.Count);
        }

        /// <summary>
        /// Checks if two lists contains the same elements.
        /// </summary>
        /// <typeparam name="T">Type of the elements</typeparam>
        /// <param name="first">First list</param>
        /// <param name="second">Second list</param>
        /// <returns>True if two lists are the same, false otherwise</returns>
        public static bool AreEqual<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first.Count() == 0 && second.Count() == 0)
            {
                return true;
            }

            return first.Count() == second.Count() && first.Where(element => second.Contains(element) == true).Any();
        }

        public static IList<T> Merge<T>(this IList<T> first, IList<T> second)
        {
            var result = first.ToList();

            result.Add(second[second.Count - 1]);

            return result;
        }
    }
}
