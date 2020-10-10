using System;
using NUnit.Framework;
using SQLiteAppTools.Models;

namespace SQLiteAppTools.Tests
{
    public class TableTests
    {
        public TableTests()
        {
        }

        [Test]
        public void a()
        {
            var table = new Table();

            Assert.NotNull(table);
        }
    }
}
