using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle.Common
{
    /// <summary>
    /// Data types for Data Elements.
    /// See A.1.1.3.1 Data Element, page 411 or Appendix page A5
    /// </summary>
    public class DataTypes
    {
        /// <summary>
        /// Signed number with implied decimal.
        /// 
        /// 'n' is a variable that represents the number of digits to the 
        /// right of the decimal point.
        /// 
        /// If n == 0, the variable may be omitted.
        /// </summary>
        public const string Numeric = "Nn";

        /// <summary>
        /// Signed decimal, may or may not contain a decimal point character.
        /// </summary>
        public const string Decimal = "R";
        public const string Identifier = "ID";
        public const string String = "AN";
        public const string Date = "DT";
        public const string Time = "TM";
        public const string Binary = "B";
    }
}