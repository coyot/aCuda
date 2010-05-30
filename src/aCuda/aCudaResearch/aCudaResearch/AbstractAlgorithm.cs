using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch
{
    /// <summary>
    /// Representation of the algorithm implementations
    /// </summary>
    public abstract class AbstractAlgorithm<T>
    {
        IDataBuilder<T> builder;

        /// <summary>
        /// Main algorithm method to execute the computation.
        /// </summary>
        public abstract void Run();
    }
}
