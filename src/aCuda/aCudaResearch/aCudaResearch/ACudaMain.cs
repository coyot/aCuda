using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using aCudaResearch.Settings;
using System.IO;
using System.Xml.Serialization;

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
            ISettingsBuilder builder = new XmlSettingsBuilder(@"D:\MGR\aCuda\src\aCuda\aCudaResearch\aCudaResearch\Data\Settings.xml");

            try
            {
                ExecutionSettings settings = builder.Build();
                ExecutionEngine engine = new ExecutionEngine(settings);

                engine.ExecuteComputation();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("There was an error during XML settings parsing.");
                Console.WriteLine(e.ToString());
            }

            Console.ReadKey();
        }
    }
}
