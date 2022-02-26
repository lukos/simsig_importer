using NUnit.Framework;
using Shouldly;
using SimsigImporterLib.Models;

namespace SimsigImporter.Tests.Unit.SpreadsheetHelper
{
    [TestFixture]
    public class ExtractAllowanceTests
    {
        [TestCase("(1)", 2, null, true)]
        [TestCase("[2]", null, 4, true)]
        [TestCase("[12]", null, 24, true)]
        [TestCase("[12h]", null, 25, true)]
        [TestCase("[12H]", null, 25, true)]
        [TestCase("[12] (9)", 18, 24, true)]
        [TestCase("[12](9)", 18, 24, true)]
        [TestCase("(7)[12]", 14, 24, true)]
        [TestCase("blah", null, null, false)]
        public void TestRegex(string input, int? expectedPath, int? expectedEng, bool isValid)
        {
            Trip trip = new Trip();
            var response = SimsigImporterLib.SpreadsheetHelper.HasAllowances(trip, input);
            response.ShouldBe(isValid);
            trip.EngAllowance.ShouldBe(expectedEng);
            trip.PathAllowance.ShouldBe(expectedPath);
        }
    }
}
