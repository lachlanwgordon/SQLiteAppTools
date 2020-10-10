using System;
using System.Collections.Generic;
using NUnit.Framework;
using SQLiteAppTools.Models;

namespace SQLiteAppTools.Tests
{
    public class RowTests
    {
        public RowTests()
        {
        }


        [Test]
        public void SearchTwoCellsWithLowerCase()
        {
            var table = new Table();
            var col1 = new Column();
            var col2 = new Column();

            var cells = new List<Cell>
            {
                new Cell("Item 1", col1),
                new Cell("Item 2", col2),
            };

            var row = new Row(cells, table);

            var match = row.Matches("item");

            Assert.AreEqual(true, match);
        }

        [Test]
        public void SearchTwoCellsWithUpperCase()
        {
            var table = new Table();
            var col1 = new Column();
            var col2 = new Column();

            var cells = new List<Cell>
            {
                new Cell("Item 1", col1),
                new Cell("Item 2", col2),
            };

            var row = new Row(cells, table);

            var match = row.Matches("Item");

            Assert.AreEqual(true, match);
        }

        [Test]
        public void SearchTwoCellsDontMatch()
        {
            var table = new Table();
            var col1 = new Column();
            var col2 = new Column();

            var cells = new List<Cell>
            {
                new Cell("Item 1", col1),
                new Cell("Item 2", col2),
            };

            var row = new Row(cells, table);

            var match = row.Matches("asdf");

            Assert.AreEqual(false, match);
        }
    }
}
