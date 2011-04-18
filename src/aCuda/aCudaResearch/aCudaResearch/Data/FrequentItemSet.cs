using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch.Data
{
    /// <summary>
    /// Klasa reprezentująca ziób częsty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FrequentItemSet<T>
    {
        private HashSet<T> itemSet = new HashSet<T>();

        /// <summary>
        /// Elementy zbioru częstego.
        /// </summary>
        public HashSet<T> ItemSet
        {
            get { return itemSet; }
        }

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="itemSet">lista elementów składających sie na zbiór częsty</param>
        public FrequentItemSet(IEnumerable<T> itemSet)
        {
            foreach (var element in itemSet)
            {
                this.itemSet.Add(element);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals((FrequentItemSet<T>)obj);
        }

        /// <summary>
        /// Sprawdza, czy dany zbiór częsty pokrywa się z innym zbiorem częstym.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true, jeżeli oba zbiory zawierają identyczny zestaw elementów, w przeciwnym razie false</returns>
        public bool Equals(FrequentItemSet<T> other)
        {
            if (other.itemSet.Count != itemSet.Count)
            {
                return false;
            }
            foreach (var item in itemSet)
            {
                if (!other.itemSet.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if (itemSet != null)
            {
                foreach (var item in itemSet)
                {
                    hashCode += item.GetHashCode();
                }
            }
            return hashCode * 397;
        }
    }
}
