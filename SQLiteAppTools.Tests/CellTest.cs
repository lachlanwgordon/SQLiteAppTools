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

    }
}
