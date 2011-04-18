﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using aCudaResearch.Algorithms;

namespace aCudaResearch
{
    public static class AlgorithmBuilder
    {
        public static DataSourceType DataSourceType { get; set; }

        public static IAlgorithm BuildAlgorithm(AlgorithmType algorithmType)
        {
            if (DataSourceType == DataSourceType.MsData)
            {
                switch (algorithmType)
                {
                    case AlgorithmType.FpGrowth:
                        return new MsWebFpGrowthAlgorithm();
                }
            }

            throw new Exception("Wrong setting!");
        }
    }
}
