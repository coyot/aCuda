using System;
using System.IO;
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
            var settingsFilePath = string.Empty;
            var printResults = true;
            var outputPath = string.Empty;

            var p = new OptionSet()
                .Add("s=", v => settingsFilePath = v)
                .Add("print=", v => printResults = bool.Parse(v))
                .Add("output=", v => outputPath = v);

            p.Parse(args);

            if (String.IsNullOrEmpty(settingsFilePath))
            {
                Console.WriteLine("You should specify the input file with program settings!");
                EndMessage();
                return;
            }

            if(!File.Exists(settingsFilePath))
            {
                Console.WriteLine("File {0} does not exists", settingsFilePath);
                EndMessage();
                return;
            }

            ISettingsBuilder builder = new XmlSettingsBuilder(settingsFilePath);

            try
            {
                var settings = builder.Build();
                if (settings != null)
                {
                    var engine = new ExecutionEngine(settings);

                    var result = engine.ExecuteComputation(printResults);
                    if (String.IsNullOrEmpty(outputPath))
                    {
                        Console.WriteLine(result.Print());
                    } 
                    else
                    {
                        var s = new FileStream(outputPath, FileMode.OpenOrCreate);
                        var writer = new StreamWriter(s);
                        
                        writer.Write(result.Print());

                        writer.Close();
                        s.Close();
                    }
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("There was an error during XML settings parsing.");
                Console.WriteLine(e.ToString());
            }
            EndMessage();
        }

        private static void EndMessage()
        {
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
        }
    }
}
