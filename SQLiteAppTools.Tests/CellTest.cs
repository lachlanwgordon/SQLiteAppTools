using System;
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

    }
}
