using PayerEDI.Data.Database.Tables;

namespace PayerEDI.Data.Database.Repositories;

public interface IPatientRepository
{
    void AddRange(IEnumerable<PatientTable> patients);

    Task<int> SaveAsync(
        IEnumerable<PatientTable> patients,
        CancellationToken cancellationToken = default
    );
}
