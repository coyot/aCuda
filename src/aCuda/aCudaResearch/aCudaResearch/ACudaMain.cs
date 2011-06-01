using System;
using Mono.Options;
using aCudaResearch.Helpers;
using aCudaResearch.Settings;

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

            int verbose = 0;
            string settingsFilePath = @"D:\MGR\aCuda\src\aCuda\aCudaResearch\aCudaResearch\Data\Settings.xml";
            OptionSet p = new OptionSet()
              .Add("s=", v => settingsFilePath = v);
            p.Parse(args);

            ISettingsBuilder builder = new XmlSettingsBuilder(settingsFilePath);

            try
            {
                var settings = builder.Build();
                if (settings != null)
                {
                    var engine = new ExecutionEngine(settings);

                    var result = engine.ExecuteComputation();
                    result.Print();
                }
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
