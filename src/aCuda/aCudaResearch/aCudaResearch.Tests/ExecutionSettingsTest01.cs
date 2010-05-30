// <copyright file="ExecutionSettingsTest.cs" company="Microsoft">Copyright © Microsoft 2010</copyright>
using System;
using System.Xml.Linq;
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
        /// <summary>Test stub for .ctor(XElement)</summary>
        [PexMethod]
        public ExecutionSettings Constructor(XElement source)
        {
            ExecutionSettings target = new ExecutionSettings(source);
            return target;
            // TODO: add assertions to method ExecutionSettingsTest.Constructor(XElement)
        }

        /// <summary>Test stub for EndNumber</summary>
        [PexMethod]
        public void EndNumberGet([PexAssumeUnderTest]ExecutionSettings target)
        {
            int result = target.EndNumber;
            // TODO: add assertions to method ExecutionSettingsTest.EndNumberGet(ExecutionSettings)
        }

        /// <summary>Test stub for MinConf</summary>
        [PexMethod]
        public void MinConfGet([PexAssumeUnderTest]ExecutionSettings target)
        {
            double result = target.MinConf;
            // TODO: add assertions to method ExecutionSettingsTest.MinConfGet(ExecutionSettings)
        }

        /// <summary>Test stub for MinSup</summary>
        [PexMethod]
        public void MinSupGet([PexAssumeUnderTest]ExecutionSettings target)
        {
            double result = target.MinSup;
            // TODO: add assertions to method ExecutionSettingsTest.MinSupGet(ExecutionSettings)
        }

        /// <summary>Test stub for StartNumber</summary>
        [PexMethod]
        public void StartNumberGet([PexAssumeUnderTest]ExecutionSettings target)
        {
            int result = target.StartNumber;
            // TODO: add assertions to method ExecutionSettingsTest.StartNumberGet(ExecutionSettings)
        }
    }
}
