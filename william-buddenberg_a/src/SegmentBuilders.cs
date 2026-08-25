using EdiFabric.Core.Model.Edi.X12;

namespace EdiMettle
{
    public class SegmentBuilders
    {
        /// <summary>
        /// Builds the ANSI X12 Interchange Header Segment.
        /// </summary>
        public static ISA BuildIsaHeaderSegment(string controlNumber,
                                                string senderId = "SENDER1",
                                                string senderQ = "14",
                                                string receiverId = "RECEIVER1",
                                                string receiverQ = "16",
                                                string ackRequested = "1",
                                                string testIndicator = "T")
        {
            return new ISA
            {
                AuthorizationInformationQualifier_1 = "00",
                AuthorizationInformation_2 = "".PadRight(10),
                SecurityInformationQualifier_3 = "00",
                SecurityInformation_4 = "".PadRight(10),
                SenderIDQualifier_5 = senderQ,
                InterchangeSenderID_6 = senderId.PadRight(15),
                ReceiverIDQualifier_7 = receiverQ,
                InterchangeReceiverID_8 = receiverId.PadRight(15),
                InterchangeDate_9 = DateTime.Now.Date.ToString("yyMMdd"),
                InterchangeTime_10 = DateTime.Now.TimeOfDay.ToString("hhmm"),
                //  Standard identifier
                InterchangeControlStandardsIdentifier_11 = "U",
                //  Interchange Version ID
                //  This is the ISA version and not the transaction sets versions
                InterchangeControlVersionNumber_12 = "00204",
                InterchangeControlNumber_13 = controlNumber.PadLeft(9, '0'),
                //  Acknowledgment Requested (0 or 1)
                AcknowledgementRequested_14 = ackRequested,
                //  Test Indicator
                UsageIndicator_15 = testIndicator,
            };
        }

        /// <summary>
        /// ANSI Functional Group Segment
        /// </summary>
        /// <param name="controlNumber">Group Segment Control Number</param>
        /// <param name="senderId">Sender ID</param>
        /// <param name="receiverId">Receiver ID</param>
        /// <param name="version">Version Number</param>
        /// <returns>Group Segment object</returns>
        public static GS BuildGs(string controlNumber, string senderId = "SENDER1", string receiverId = "RECEIVER1", string version = "004010")
        {
            GS newGroup = new()
            {
                CodeIdentifyingInformationType_1 = "IN",
                SenderIDCode_2 = senderId,
                ReceiverIDCode_3 = receiverId,
                Date_4 = DateTime.Now.Date.ToString("yyMMdd"),
                Time_5 = DateTime.Now.TimeOfDay.ToString("hhmm"),

                //  Must be unique to both partners for this interchange
                GroupControlNumber_6 = controlNumber.PadLeft(9, '0'),

                //  Responsible Agency Code
                TransactionTypeCode_7 = "X",
                VersionAndRelease_8 = version
            };

            return newGroup;
        }

    }
}
