using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch
{
    /// <summary>
    /// This class will be used to execute research experiments.
    /// 
    /// This will also include time measuring process, loading data, execution of proper algorithms.
    /// </summary>
    public class ExecutionEngine
    {
        private ExecutionSettings settings;

        public ExecutionEngine(ExecutionSettings settings)
        {
            this.settings = settings;
        }
        /// <summary>
        /// Engine main method to execute computation process.
        /// </summary>
        /// <param name="args">Parameters to the execution</param>
        public void ExecuteComputation()
        {
            Console.WriteLine("Computation will be executed.");
            Console.WriteLine(this.settings.ToString());
        }
    }
}
