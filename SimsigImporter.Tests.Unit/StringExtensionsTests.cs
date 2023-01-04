﻿using NUnit.Framework;
using SimsigImporterLib.Helpers;
using Shouldly;

namespace SimsigImporter.Tests.Unit
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase("SO", "S", true)]
        [TestCase("SO", "SUN", false)]
        [TestCase("SO", "Th", false)]
        [TestCase("SX", "S", false)]
        [TestCase("SX", "F", true)]
        [TestCase("SX", "SUN", false)]
        [TestCase("SUN", "S", false)]
        [TestCase("SUN", "F", false)]
        [TestCase("SUN", "SUN", true)]
        [TestCase("MSO", "M", true)]
        [TestCase("MSO", "S", true)]
        [TestCase("MSO", "Th", false)]
        [TestCase("MSX", "M", false)]
        [TestCase("MSX", "S", false)]
        [TestCase("MSX", "Th", true)]
        public void TestContainsDay(string input, string day, bool expectedOutput)
        {
            input.ContainsDay(day).ShouldBe(expectedOutput);
        }

        // Note that failed conversion return 0
        [TestCase("0010", 600)]
        [TestCase("00101", 0)]
        [TestCase("random", 0)]
        [TestCase("1610", 58200)]
        [TestCase("16:10", 58200)]
        [TestCase("1610H", 58230)]
        [TestCase("16:10H", 58230)]
        public void TestToSimsigTime(string input, int result)
        {
            input.ToSimsigTime().ShouldBe(result);
        }

        [TestCase(1, "A")]
        [TestCase(26, "Z")]
        [TestCase(27, "AA")]
        [TestCase(52, "AZ")]
        [TestCase(675, "YY")]
        [TestCase(676, "YZ")]
        [TestCase(677, "ZA")]
        [TestCase(702, "ZZ")]
        [TestCase(703, "AAA")]
        public void TestToExcelColumn(int column, string result)
        {
            column.ToExcelColumn().ShouldBe(result);
        }
    }
}
