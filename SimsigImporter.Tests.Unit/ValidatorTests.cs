using NUnit.Framework;
using Shouldly;
using SimsigImporterLib.Helpers;
using System;
using System.Text.RegularExpressions;

namespace SimsigImporter.Tests.Unit
{
    public class ValidatorTests
    {
        [TestCase("SO", true)]
        [TestCase("SX", true)]
        [TestCase("S", false)]
        [TestCase("SUN", true)]
        [TestCase("MTWO", true)]
        [TestCase("MThSX", true)]
        [TestCase("O", false)]
        [TestCase("SUNO", false)]
        public void TestDaysFormat(string input, bool expectedOutput)
        {
            Validators.VerifyDayCode(input).ShouldBe(expectedOutput);
        }


        [TestCase("(1)", 1, null)]
        [TestCase("[2]", null, 2)]
        [TestCase("[12]", null, 12)]
        [TestCase("[12] (9)", 9, 12)]
        [TestCase("[12](9)", 9, 12)]
        [TestCase("(7)[12]", 7, 12)]
        public void TestTimingExtract(string input, int? allowance, int? eng)
        {
            var content = Regex.Match(input, @"(\(([0-9]{1,2})\))");
            content.Groups[2].Value.ToNullableInt().ShouldBe(allowance);

            content = Regex.Match(input, @"(\[([0-9]{1,2})\])");
            content.Groups[2].Value.ToNullableInt().ShouldBe(eng);
        }
    }
}