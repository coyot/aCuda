using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch
{
    /// <summary>
    /// Main class of the aCuda project.
    /// 
    /// EXECUTION: [TODO]
    /// </summary>
    public class ACudaMain
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("This is my master thesis research app.");
            ExecutionEngine Engine = new ExecutionEngine();
            AlgorithmType type = Enum.Parse(typeof(AlgorithmType), args[0]);

            Engine.ExecuteComputation(args);

            Console.ReadKey();
        }
    }
}
