using System;
using System.Security.Cryptography;
using System.Text;

namespace SimsigImporterLib.Helpers
{
    /// <summary>
    /// Helper class for methods related to security or encryption
    /// </summary>
    internal class Encryption
    {
        internal static readonly char[] chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        /// <summary>
        /// Generate an id of the given length with the chars from the chars array
        /// </summary>
        /// <param name="length">The number of characters to return</param>
        /// <returns>A randomnly generate string</returns>
        /// <remarks>from https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings</remarks>
        public static string GenerateId(int length)
        {
            byte[] data = new byte[4 * length];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }
    }
}
