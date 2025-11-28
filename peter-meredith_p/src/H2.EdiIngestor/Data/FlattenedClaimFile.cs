using EdiFabric.Templates.Hipaa5010;

namespace H2.EdiIngestor;

public class FlattenedClaimFile
{
    public Loop_1000A_837P? Loop1000A { get; set; }
    public Loop_1000B_837P? Loop1000B { get; set; }

    public Loop_2000A_837P? Loop2000A { get; set; }
    public Loop_2000B_837P? Loop2000B { get; set; }
    public Loop_2000C_837P? Loop2000C { get; set; }

    public Loop_2300_837P? Loop2300 { get; set; }
    public IEnumerable<Loop_2400_837P>? Loop2400 { get; set; }

    public FlattenedClaimFile ShallowCopy()
    {
        return (FlattenedClaimFile)this.MemberwiseClone();
    }
}
