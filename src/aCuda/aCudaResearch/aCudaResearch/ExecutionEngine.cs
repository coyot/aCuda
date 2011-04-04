using System;
using System.Collections.Generic;
using aCudaResearch.FpGrowth;
using aCudaResearch.FpGrowth.Data.MsWeb;

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
        private Dictionary<AlgorithmType, AbstractAlgorithm<MsInstance<int>>> _msInstanceAlgorithms;

        public ExecutionSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public ExecutionEngine(ExecutionSettings settings)
        {
            _settings = settings;
            _msInstanceAlgorithms = new Dictionary<AlgorithmType, AbstractAlgorithm<MsInstance<int>>>();
        }

        /// <summary>
        /// Engine main method to execute computation process.
        /// </summary>
        public void ExecuteComputation()
        {
            //!+ here the computation time measuring should be placed!!!

            foreach (var algorithm in _settings.Algorithms)
            {
                if (!_msInstanceAlgorithms.ContainsKey(algorithm))
                {
                    // here the builder should be introduced!
                    _msInstanceAlgorithms.Add(algorithm, new FpGrowthAlgorithm());
                }
            }

            Console.WriteLine("Computation will be executed.");
            Console.WriteLine(_settings.ToString());
        }
    }
}
