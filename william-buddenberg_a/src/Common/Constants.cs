using System;
using System.Collections.Generic;
using System.Text;

namespace EdiMettle.Common
{
    public static class Constants
    {
        public const int ControlNumberLength = 9;
        public const char ControlNumPadding = '0';
        public const string InterchangeDateFormat = "yyMMdd";
        public const string InterchangeTimeFormat = "hhmm";
        public const string DateTimeFormat = InterchangeDateFormat + InterchangeTimeFormat;

        public const int AuthorizationInfoLength = 10;
        public const int SecurityInfoLength = 10;
        public const int SenderIdLength = 15;
        public const int ReceiverIdLength = 15;
        public const string TestIndicator = "T";
    }
}
