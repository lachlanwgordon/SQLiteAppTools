using System;
using System.Collections.Generic;
using NUnit.Framework;
using SQLiteAppTools.Models;

namespace SQLiteAppTools.Tests
{
    public class CellTest
    {
        public CellTest()
        {
        }

        [Test]
        public void TestCellWithString()
        {
            var column = new Column();
            var testString = "TestString";
            var cell = new Cell(testString, column);

            Assert.AreEqual(testString, cell.ToString());
        }

        [Test]
        public void TestCellWithInt()
        {
            var column = new Column();
            var testInt = 10;
            var cell = new Cell(testInt, column);

            Assert.AreEqual("10", cell.ToString());
        }

        public static IEnumerable<TestCaseData> TestData
        {
            get
            {
                yield return new TestCaseData("TestString", "TestString");
                yield return new TestCaseData(10, "10");
                yield return new TestCaseData(10.3, "10.3");
                yield return new TestCaseData(new DateTime(2020,10,10), "2020-10-10T00:00:00");
            }
        }

        [TestCaseSource(typeof(CellTest), nameof(TestData))]
        public void TestCellWithTypes(object item, string expected)
        {
            var column = new Column();
            var cell = new Cell(item, column);

            Assert.AreEqual(expected, cell.ToString());
        }

        public static IEnumerable<TestCaseData> URLTests
        {
            get
            {
                yield return new TestCaseData("http://google.com", true);
                yield return new TestCaseData("https://google.com", true);
                yield return new TestCaseData("https://twitter.com/home", true);
                yield return new TestCaseData("https://pbs.twimg.com/profile_images/1278472140673572864/qHD60s7Z_400x400.jpg", true);
                yield return new TestCaseData("https", false);
                yield return new TestCaseData("3.5", false);
                yield return new TestCaseData(3.5, false);
                yield return new TestCaseData(new DateTime(), false);
                yield return new TestCaseData(new Guid(), false);
            }
        }

        [TestCaseSource(typeof(CellTest), nameof(URLTests))]
        public void TestURLs(object url, bool expected)
        {
            var column = new Column();
            var cell = new Cell(url, column);
            var isUrl = cell.IsUrl;
            Assert.AreEqual(expected, isUrl);
        }


    }
}
