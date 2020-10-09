using System;
using System.Collections.Generic;
using NUnit.Framework;
using SQLiteAppTools.Extensions;
using static SQLite.SQLite3;

namespace SQLiteAppTools.Tests
{
    public class ColTypeCLRTypeTests
    {
        public static IEnumerable<TestCaseData> TestTypesData
        {
            get
            {
                yield return new TestCaseData(ColType.Blob, typeof(Byte[]));
                yield return new TestCaseData(ColType.Float, typeof(float));
                yield return new TestCaseData(ColType.Integer, typeof(int));
                yield return new TestCaseData(ColType.Null, typeof(object));
                yield return new TestCaseData(ColType.Text, typeof(string));
            }
        }

        [TestCaseSource(typeof(ColTypeCLRTypeTests), nameof(TestTypesData))]
        public void TestTypes(ColType colType, Type expected)
        {
            var type = SQLiteExtensions.GetTypeFromColType(colType);
            Assert.AreEqual(expected, type);
        }

    }
}
