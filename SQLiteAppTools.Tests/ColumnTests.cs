using System;
using NUnit.Framework;
using SQLiteAppTools.Models;

namespace SQLiteAppTools.Tests
{
    public class ColumnTests
    {
        public ColumnTests()
        {
        }

        [Test]
        public void CheckLengthForName()
        {
            var column = new Column();
            column.Name = "Test";

            Assert.AreEqual(4, column.MaxLength);

        }

        [Test]
        public void UpdateMaxLength()
        {
            var column = new Column();
            column.Name = "Test";

            column.CheckForMaxLength(Guid.NewGuid().ToString());

            Assert.AreEqual(36, column.MaxLength);
        }
    }
}
