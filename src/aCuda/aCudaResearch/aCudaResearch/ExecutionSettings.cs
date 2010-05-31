using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;

namespace aCudaResearch
{
    [Serializable]
    public class ExecutionSettings
    {
        private double minSup;
        private double minConf;
        private int startNumber;
        private int endNumber;

        public ExecutionSettings()
        {
            Algorithms = new List<AlgorithmType>();
        }

        public double MinSup
        {
            get
            {
                Contract.Ensures(Contract.Result<double>() >= 0);

                return minSup;
            }

            set
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

            set
            {
                Contract.Requires(value >= 0);
                minConf = value;
            }
        }

        /// <summary>
        /// Initial number of transactions which will be taken into execution.
        /// </summary>
        public int StartNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return startNumber;
            }

            set
            {
                Contract.Requires(value >= 0);
                startNumber = value;
            }
        }

        /// <summary>
        /// Max number of transactions which will be taken into execution.
        /// </summary>
        public int EndNumber
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return endNumber;
            }

            set
            {
                Contract.Requires(value >= 0);
                endNumber = value;
            }
        }

        /// <summary>
        /// Path to the file with transactions.
        /// </summary>
        public string DataSourcePath { get; set; }

        [XmlArray(ElementName="Algorithms")]
        [XmlArrayItem(ElementName="Algorithm")]
        public List<AlgorithmType> Algorithms { get; set; }

        /// <summary>
        /// Override base ToString() method with one which is more useful.
        /// </summary>
        /// <returns>Settings description</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Data Source: ").Append(DataSourcePath);
            builder.Append("\nMinSup: ").Append(MinSup);
            builder.Append("\nMinConf: ").Append(MinConf);
            builder.Append("\n\tStart Number: ").Append(StartNumber);
            builder.Append("\n\tEnd Number: ").Append(EndNumber);
            builder.Append("\nAlgorithms: ");

            foreach (AlgorithmType algorithm in Algorithms)
            {
                builder.Append("\n\t - ").Append(algorithm);
            }

            return builder.ToString();
        }
    }
}
