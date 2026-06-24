namespace EDI.Common.Constants
{
    public enum EdiDocumentType
    {
        _837 = 837,
        _835 = 835,
        _270 = 270,
        _271 = 271,
        T837D = 8371,
        T837I = 8372,
        T837P = 8373
    }

    public enum EdiProcessingStatus
    {
        Pending,
        Processing,
        Completed,
        Failed
    }
}
