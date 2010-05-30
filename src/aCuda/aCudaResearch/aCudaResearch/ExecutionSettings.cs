using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace aCudaResearch
{
    public class ExecutionSettings
    {
        public double MinSup { get; private set; }
        public double MinConf { get; private set; }
        public int StartNumber { get; private set; }
        public int EndNumber { get; private set; }

        public ExecutionSettings(XElement source)
        {
            MinSup = 
                (from s in source.Descendants("generalSettings")
                 select (double)s.Element("minSup")).FirstOrDefault();
            MinConf = 0;
            StartNumber = 10;
            EndNumber = 100;
        }
    }
}
