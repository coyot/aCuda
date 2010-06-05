using System;

namespace aCudaResearch
{
    /// <summary>
    /// This class will be used to execute research experiments.
    /// 
    /// This will also include time measuring process, loading data, execution of proper algorithms.
    /// </summary>
    public class ExecutionEngine
    {
        private readonly ExecutionSettings _settings;

        public ExecutionEngine(ExecutionSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Engine main method to execute computation process.
        /// </summary>
        public void ExecuteComputation()
        {
            Console.WriteLine("Computation will be executed.");
            Console.WriteLine(_settings.ToString());
        }
    }
}
