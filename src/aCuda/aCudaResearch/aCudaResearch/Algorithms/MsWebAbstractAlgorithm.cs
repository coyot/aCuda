using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aCudaResearch.Data.MsWeb;

namespace aCudaResearch.Algorithms
{
    /// <summary>
    /// Representation of the algorithm which uses MSWeb Data as the input.
    /// </summary>
    public abstract class MsWebAbstractAlgorithm : IAlgorithm
    {
        internal MsDataBuilder builder;

        public abstract void Run(ExecutionSettings executionSettings);
    }
}
