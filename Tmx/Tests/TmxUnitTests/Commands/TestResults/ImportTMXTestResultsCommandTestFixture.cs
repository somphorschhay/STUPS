﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 6/3/2013
 * Time: 1:58 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace TmxUnitTests.Commands.TestResults
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using MbUnit.Framework;
    // using NUnit.Framework;
    using Tmx.Core;
    using Tmx.Interfaces.TestStructure;
    
    /// <summary>
    /// Description of ImportTmxTestResultsCommandTestFixture.
    /// </summary>
    [MbUnit.Framework.TestFixture][NUnit.Framework.TestFixture]
    public class ImportTmxTestResultsCommandTestFixture
    {
        List<ITestSuite> _testSuites;
        
        public ImportTmxTestResultsCommandTestFixture()
        {
            UnitTestingHelper.PrepareUnitTestDataStore();
            _testSuites = new List<ITestSuite>();
        }
        
        [MbUnit.Framework.SetUp][NUnit.Framework.SetUp]
        public void SetUp()
        {
            UnitTestingHelper.PrepareUnitTestDataStore();
            _testSuites = new List<ITestSuite>();
        }
        
        [MbUnit.Framework.Test][NUnit.Framework.Test]
        public void Should_import_one_test_suite()
        {
            const string testSuiteId = "1";
            const string testSuiteName = "name";
            var rootNode =
                GIVEN_root_node(
                    GIVEN_testSuite(testSuiteId, testSuiteName, "pl001", Guid.NewGuid()));
            
            WHEN_importing_test_results(rootNode);
            
            THEN_there_are_number_of_test_suites(1);
            Assert.AreEqual(testSuiteId, _testSuites[0].Id);
            Assert.AreEqual(testSuiteName, _testSuites[0].Name);
        }
        
        [MbUnit.Framework.Test][NUnit.Framework.Test]
        public void Should_import_two_test_suites_with_the_same_platform()
        {
            const string testSuiteId01 = "1";
            const string testSuiteName01 = "name";
            const string testSuiteId02 = "2";
            const string testSuiteName02 = "name2";
            var rootNode =
                GIVEN_root_node(
                    GIVEN_testSuite(testSuiteId01, testSuiteName01, "pl001", Guid.NewGuid()),
                    GIVEN_testSuite(testSuiteId02, testSuiteName02, "pl001", Guid.NewGuid()));
            
            WHEN_importing_test_results(rootNode);
            
            THEN_there_are_number_of_test_suites(2);
            Assert.AreEqual(testSuiteId01, _testSuites[0].Id);
            Assert.AreEqual(testSuiteName01, _testSuites[0].Name);
            Assert.AreEqual(testSuiteId02, _testSuites[1].Id);
            Assert.AreEqual(testSuiteName02, _testSuites[1].Name);
        }
        
        [MbUnit.Framework.Test][NUnit.Framework.Test]
        public void Should_merge_two_identical_test_suites()
        {
            const string testSuiteId = "1";
            const string testSuiteName = "name";
            var rootNode =
                GIVEN_root_node(
                    GIVEN_testSuite(testSuiteId, testSuiteName, "pl001", Guid.NewGuid()),
                    GIVEN_testSuite(testSuiteId, testSuiteName, "pl001", Guid.NewGuid()));
            
            WHEN_importing_test_results(rootNode);
            
            THEN_there_are_number_of_test_suites(1);
            Assert.AreEqual(testSuiteId, _testSuites[0].Id);
            Assert.AreEqual(testSuiteName, _testSuites[0].Name);
        }
        
        [MbUnit.Framework.Test][NUnit.Framework.Test]
        public void Should_import_two_test_suites_with_different_platforms()
        {
            const string testSuiteId01 = "1";
            const string testSuiteName01 = "name";
            const string testSuitePlatform01 = "pl001";
            var guid01 = Guid.NewGuid();
            const string testSuiteId02 = "2";
            const string testSuiteName02 = "name2";
            const string testSuitePlatform02 = "pl002";
            var guid02 = Guid.NewGuid();
            var rootNode =
                GIVEN_root_node(
                    GIVEN_testSuite(testSuiteId01, testSuiteName01, testSuitePlatform01, guid01),
                    GIVEN_testSuite(testSuiteId02, testSuiteName02, testSuitePlatform02, guid02));
            
            WHEN_importing_test_results(rootNode);
            
            THEN_there_are_number_of_test_suites(2);
            Assert.AreEqual(testSuiteId01, _testSuites[0].Id);
            Assert.AreEqual(testSuiteName01, _testSuites[0].Name);
            Assert.AreEqual(Guid.Empty, _testSuites[0].PlatformUniqueId);
            Assert.AreEqual(testSuiteId02, _testSuites[1].Id);
            Assert.AreEqual(testSuiteName02, _testSuites[1].Name);
            Assert.AreEqual(Guid.Empty, _testSuites[1].PlatformUniqueId);
        }
        
        // ============================================================================================================================
        XElement GIVEN_root_node(params XElement[] elements)
        {
            return new XElement(
                "suites",
                elements);
        }
        
        XElement GIVEN_testSuite(string id, string name, string platformId, Guid guid)
        {
            return new XElement(
                "suite",
                new XAttribute("uniqueId", guid),
                new XAttribute("id", id),
                new XAttribute("name", name),
                new XAttribute("status", "PASSED"),
                new XAttribute("platformId", platformId));
        }
        
        void WHEN_importing_test_results(object rootElement)
        {
            var xDoc = new XDocument(rootElement);
            var testResultsImporter = new TestResultsImporter();
            testResultsImporter.ImportTestPlatformFromXdocument(xDoc);
            _testSuites = testResultsImporter.ImportTestResultsFromXdocument(xDoc);
        }
        
        void THEN_there_are_number_of_test_suites(int number)
        {
            Assert.AreEqual(number, _testSuites.Count);
        }
    }
}
