using PayerEDI.Data.Models;

namespace PayerEDI.Data.Services;

public interface IEdiProcessor
{
    public List<ProcessedEdiTransaction> ProcessEdi(Stream ediStream);
}
