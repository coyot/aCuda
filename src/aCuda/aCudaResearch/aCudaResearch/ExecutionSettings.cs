using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics.Contracts;

namespace aCudaResearch
{
    public class ExecutionSettings
    {
        private double minSup;
        private double minConf;
        private int startNumber;
        private int endNumber;
        private string dataPath;

        public double MinSup
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() >= 0);

                return minSup;
            }

            private set
            {
                Contract.Requires(value >= 0);
                minSup = value;
            }
        }

        public double MinConf
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() >= 0);

                return minConf;
            }

            private set
            {
                Contract.Requires(value >= 0);
                minConf = value;
            }
        }

        public int StartNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return startNumber;
            }

            private set
            {
                Contract.Requires(value >= 0);
                startNumber = value;
            }
        }

        public int EndNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return endNumber;
            }

            private set
            {
                Contract.Requires(value >= 0);
                endNumber = value;
            }
        }


        public string DataPath
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() !=null);

                return dataPath;
            }

            private set
            {
                Contract.Requires(value != null);
                dataPath = value;
            }
        }

        public ExecutionSettings(XElement source)
        {
            Contract.Requires(source != null);

            MinSup = -0.2;
            MinConf = 0;
            StartNumber = 10;
            EndNumber = 100;
        }
    }
}
