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
    }
}