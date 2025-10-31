using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X12EDI.Data.Entities
{
    public class EdiEnvelope
    {
        #region Public Properties

        public string? CodeIdentifyingInformationType { get; set; }
        public EdiFile EdiFile { get; set; } = default!;
        public int EdiFileId { get; set; }
        public string? GroupControlNumber { get; set; }
        public int Id { get; set; }

        public string? InterchangeControlNumber { get; set; }  // ISA13
        public string? ReceiverId { get; set; }
        public string? SenderId { get; set; }                  // ISA06

        // ISA08
        // GS06
        // GS01
        public DateTime Timestamp { get; set; }

        #endregion Public Properties
    }

    public class EdiError
    {
        #region Public Properties

        public EdiFile EdiFile { get; set; } = null!;
        public int EdiFileId { get; set; }
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;

        #endregion Public Properties
    }

    public class EdiFile
    {
        #region Public Properties

        public EdiEnvelope? Envelope { get; set; }
        public ICollection<EdiError> Errors { get; set; } = new List<EdiError>();
        public int Id { get; set; }
        public string Identifier { get; set; } = string.Empty;
        public DateTime IngestedAt { get; set; }
        public ICollection<EdiTransaction> Transactions { get; set; } = new List<EdiTransaction>();

        #endregion Public Properties
    }

    public class EdiTransaction
    {
        #region Public Properties

        public string? Checksum { get; set; }
        public string? ControlNumber { get; set; }
        public EdiFile EdiFile { get; set; } = default!;
        public int EdiFileId { get; set; }
        public int Id { get; set; }
        public string? Raw { get; set; }
        public string? TransactionType { get; set; }

        #endregion Public Properties
    }
}