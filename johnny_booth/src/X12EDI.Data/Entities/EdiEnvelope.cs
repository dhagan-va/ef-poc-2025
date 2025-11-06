using EdiFabric.Core.Model.Edi.X12;

namespace X12EDI.Data.Entities
{
    public class EdiEnvelope
    {
        #region Public Constructors

        public EdiEnvelope(GS gs)
        {
            MapGS(gs);
        }

        public EdiEnvelope(ISA isa)
        {
            MapISA(isa);
        }

        #endregion Public Constructors

        #region Public Properties

        public string? CodeIdentifyingInformationType { get; set; }

        public EdiFile EdiFile { get; set; } = default!;

        public int EdiFileId { get; set; }

        public string? GroupControlNumber { get; set; }

        public int Id { get; set; }

        public string? InterchangeControlNumber { get; set; }

        // ISA13
        public string? ReceiverId { get; set; }

        public string? SenderId { get; set; }

        // ISA08
        // GS06
        // GS01
        public DateTime Timestamp { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void MapGS(GS gs)
        {
            GroupControlNumber = gs.GroupControlNumber_6;
            CodeIdentifyingInformationType = gs.CodeIdentifyingInformationType_1;
        }

        public void MapISA(ISA isa)
        {
            InterchangeControlNumber = isa.InterchangeControlNumber_13;
            SenderId = isa.InterchangeSenderID_6;
            ReceiverId = isa.InterchangeReceiverID_8;
            Timestamp = DateTime.UtcNow;
        }

        #endregion Public Methods

        // ISA06
    }
}