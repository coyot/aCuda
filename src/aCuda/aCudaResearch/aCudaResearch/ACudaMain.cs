using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;

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
            XElement xmlSettings = XElement.Load(@"D:\MGR\aCuda\src\aCuda\aCudaResearch\aCudaResearch\Data\Settings.xml");
            
            //ExecutionSettings settings = new ExecutionSettings(xmlSettings);
            //ExecutionEngine Engine = new ExecutionEngine(settings);
            //AlgorithmType type = Enum.Parse(typeof(AlgorithmType), args[0]);

            //Engine.ExecuteComputation();

            Console.ReadKey();
        }
    }
}
