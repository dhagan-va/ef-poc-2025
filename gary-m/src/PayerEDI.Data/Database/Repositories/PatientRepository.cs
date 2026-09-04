using PayerEDI.Data.Database.Tables;

namespace PayerEDI.Data.Database.Repositories;

public class PatientRepository(PayerEdiDbContext context) : IPatientRepository
{
    public void AddRange(IEnumerable<PatientTable> patients) => context.Patients.AddRange(patients);

    public Task<int> SaveAsync(
        IEnumerable<PatientTable> patients,
        CancellationToken cancellationToken = default
    )
    {
        AddRange(patients);
        return context.SaveChangesAsync(cancellationToken);
    }
}
