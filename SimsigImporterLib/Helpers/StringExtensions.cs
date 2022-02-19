using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    }
}
