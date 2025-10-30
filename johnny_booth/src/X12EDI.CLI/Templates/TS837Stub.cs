using EdiFabric.Core.Annotations.Edi;
using EdiFabric.Core.Annotations.Validation;
using EdiFabric.Core.Model.Edi;
using EdiFabric.Core.Model.Edi.X12;
using EdiFabric.Templates.Hipaa5010;
using System.Collections.Generic;

namespace X12EDI.Templates
{
    [Message("X12", "005010X222A1", "837")]
    public class TS837Stub : EdiMessage
    {
        [Pos(1)]
        public ST ST { get; set; }

        [Pos(2)]
        public BHT BHT { get; set; }   // required here

        [Pos(3)]
        [ListCount(100)]
        public List<Loop2300Stub> Loop2300 { get; set; }

        [Pos(4)]
        public SE SE { get; set; }
    }

    [Group(typeof(CLMStub))]
    public class Loop2300Stub
    {
        [Pos(1)]
        public CLMStub CLM { get; set; }
    }

    [Segment("CLM")]
    public class CLMStub
    {
        [Pos(1)]
        public string ClaimSubmitterIdentifier { get; set; }

        [Pos(2)]
        public decimal? MonetaryAmount { get; set; }
    }
}
