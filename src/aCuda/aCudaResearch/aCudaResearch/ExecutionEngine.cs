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
        public ExecutionEngine()
        {
        }
        /// <summary>
        /// Engine main method to execute computation process.
        /// </summary>
        /// <param name="args">Parameters to the execution</param>
        public void ExecuteComputation(string[] args)
        {
            Console.WriteLine("Computation will be executed.");
        }
    }
}
