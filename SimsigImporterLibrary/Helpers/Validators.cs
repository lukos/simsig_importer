using System.Text.RegularExpressions;

namespace SimsigImporterLib.Helpers
{
    /// <summary>
    /// Validation helpers
    /// </summary>
    public static class Validators
    {
        private static Regex daysCode = new Regex("^((M|T|W|Th|F|S){1,6}(O|X))|SUN$", RegexOptions.Compiled);

        /// <summary>
        /// Verify the code for the day restriction on a service. Only needs to validate codes, not blanks.
        /// </summary>
        /// <param name="input">The days input</param>
        /// <returns>True if the code represents a valid set of days, otherwise false</returns>
        public static bool VerifyDayCode(string input)
        {
            return input.IsPresent() && daysCode.IsMatch(input);
        }
    }
}
