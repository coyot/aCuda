// <copyright file="ExecutionSettingsTest.cs" company="Microsoft">Copyright © Microsoft 2010</copyright>
using System;
using aCudaResearch;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace aCudaResearch
{
    /// <summary>This class contains parameterized unit tests for ExecutionSettings</summary>
    [PexClass(typeof(ExecutionSettings))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ExecutionSettingsTest
    {
        /// <summary>Test stub for CountDummy(Int32, Int32)</summary>
        [PexMethod]
        public int CountDummy(
            [PexAssumeUnderTest]ExecutionSettings target,
            int i,
            int j
        )
        {
            int result = target.CountDummy(i, j);
            return result;
            // TODO: add assertions to method ExecutionSettingsTest.CountDummy(ExecutionSettings, Int32, Int32)
        }
    }
}
