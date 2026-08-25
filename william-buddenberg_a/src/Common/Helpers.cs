using System.Globalization;

namespace EdiMettle.Common
{
    public static class Helpers
    {
        /// <summary>
        /// Prepends a Control Number with 0's so that it is 9 digits long.
        /// </summary>
        /// <returns>9 digit control number</returns>
        public static string GetPaddedControlNumber(int controlNumber)
        {
            string paddedNum = controlNumber.ToString().PadLeft(9, '0');

            return paddedNum;
        }

        /// <summary>
        /// Returns a string 1 for true, 0 for false.
        /// </summary>
        /// <param name="truthyness">Boolean to convert</param>
        /// <returns>String containing either 1 or 0</returns>
        public static string BoolToBitString(bool truthyness)
        {
            string boolBit = truthyness ? "1" : "0";
            return boolBit;
        }

        /// <summary>
        /// Converts EDI date and time to DateTime.
        /// </summary>
        /// <param name="dateCode">Raw date stamp</param>
        /// <param name="timeCode">Raw time stamp</param>
        /// <returns>DateTime object</returns>
        public static DateTime GetDateTimeFromEdi(string dateCode, string timeCode)
        {
            DateTime parsedTime = DateTime.ParseExact(dateCode + timeCode, Constants.DateTimeFormat, CultureInfo.CurrentCulture);

            return parsedTime;
        }
    }
}
