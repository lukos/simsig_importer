using System;
using System.Text.RegularExpressions;

namespace SimsigImporterLib.Helpers
{
    public static class StringExtensions
    {
        public static bool IsMissing(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool IsPresent(this string input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }

        public static string ToExcelColumn(this int columnNumber)
        {
            return columnNumber > 26 ? Convert.ToChar(64 + (columnNumber / 26)).ToString() + Convert.ToChar(64 + (columnNumber % 26)) : Convert.ToChar(64 + columnNumber).ToString();
        }

        private static Regex simsigTime = new Regex("[0-9]{2}:?[0-9]{2}", RegexOptions.Compiled);

        public static int ToSimsigTime(this string input)
        {
            if (!simsigTime.IsMatch(input))
            {
                return 0;
            }
            // If in user-friendly mode like 20:00 do it slightly differently than if just numbers like 2000
            if (input.Contains(":"))
            {
                return Convert.ToInt32(input.Split(':')[0]) * 3600 + Convert.ToInt32(input.Split(':')[1]) * 60;
            }
            return Convert.ToInt32(input.Substring(0, 2)) * 3600 + Convert.ToInt32(input.Substring(2, 2)) * 60;
        }

        /// <summary>
        /// Converts the normal name of a day to the timetable equivalent e.g. Saturday => S
        /// </summary>
        /// <param name="input">The day to convert</param>
        /// <returns>A string representation</returns>
        public static string ToDayCode(this string input)
        {
            switch(input)
            {
                case "Monday":
                    return "M";
                case "Tuesday":
                    return "T";
                case "Wednesday":
                    return "W";
                case "Thursday":
                    return "Th";
                case "Friday":
                    return "F";
                case "Saturday":
                    return "S";
                case "Sunday":
                    return "SUN";
                default:
                    throw new NotImplementedException($"Unexpected day [{input}] in ToDayCode");
            }
        }

        /// <summary>
        /// Whether the day code should allow running on the given day e.g. T is allowed for TO but not TX
        /// </summary>
        /// <param name="input">The set of days that the working can run on e.g. MSO</param>
        /// <param name="day">The specific day code we are querying e.g. S</param>
        /// <returns>True if the days are blank or the code permits running</returns>
        public static bool ContainsDay(this string input, string day)
        {
            if (input.IsMissing())
            {
                return true;    // Blank days means unrestricted
            }
            if (input == "SUN")
            {
                // Sunday is special case and is tabulated separately than the rest of the timetable
                return day == "SUN";
            }
            else if (input.Contains(day))
            {
                // If the day is in the code, then it must end with "only"
                return input.EndsWith("O");
            }
            else
            {
                // If the day is not in the code then it must end with "excepted"
                return day != "SUN" && input.EndsWith("X");
            }
        }
    }
}
