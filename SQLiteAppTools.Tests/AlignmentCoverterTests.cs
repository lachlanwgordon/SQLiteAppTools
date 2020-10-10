using System;
using NUnit.Framework;
using SQLiteAppTools.Converters;
using Xamarin.Forms;

namespace SQLiteAppTools.Tests
{
    public class AlignmentCoverterTests
    {
        [Test]
        public void Test()
        {
            var type = typeof(int);
            var expected = TextAlignment.End;

            var alignment = TypeToAlignmentConverter.Instance.Convert(type, null, null, null);

            Assert.AreEqual(expected, alignment);
        }

    }
}
