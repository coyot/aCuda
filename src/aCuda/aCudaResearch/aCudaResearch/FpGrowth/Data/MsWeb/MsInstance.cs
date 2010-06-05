using System;
using System.Collections.Generic;

namespace aCudaResearch.FpGrowth.Data.MsWeb
{
    public class MsInstance<T>
    {
        public Dictionary<T, MsElement> Attributes { get; set; }

        public Dictionary<T, int[]> Database { get; set; }

        public void AddAttribute(string textLine)
        {
            if (textLine == null) throw new ArgumentNullException("textLine");
            System.Diagnostics.Contracts.Contract.EndContractBlock();

        }

        public void AddEntry(T id, int[] values)
        {
            Database.Add(id, values);
        }
    }
}
