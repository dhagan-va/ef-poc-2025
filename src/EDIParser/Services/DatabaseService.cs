using EdiFabric.Core.Model.Edi;
using EdiFabric.Templates.Hipaa5010;
using EdiParser.Entities;
using Microsoft.Extensions.Logging;

namespace EdiParser.Services;

public interface IDatabaseService
{
    void Save837<T>(List<T> ediTransactions, ClaimTypeEnum claimType) where T : EdiMessage;
    Task Save837Async<T>(List<T> ediTransactions, ClaimTypeEnum claimType) where T : EdiMessage;
}

public class DatabaseService : IDatabaseService
{
    private readonly ILogger<Application> _logger;
    private readonly IEdiDBContext _context;
    
    public DatabaseService(ILogger<Application> logger, IEdiDBContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    /// <summary>
    /// Synchronous version of the save
    /// </summary>
    /// <param name="ediTransactions"></param>
    /// <param name="claimType"></param>
    /// <typeparam name="T"></typeparam>
    public void Save837<T>(List<T> ediTransactions, ClaimTypeEnum claimType) where T : EdiMessage
    {
        _logger.LogInformation($"Commiting {ediTransactions.Count} edi data...");
        try
        {
            if(claimType == ClaimTypeEnum.Professional) _context.TS837P.AddRangeAsync(ediTransactions.OfType<TS837P>());
            if(claimType == ClaimTypeEnum.Institutional) _context.TS837I.AddRangeAsync(ediTransactions.OfType<TS837I>());
            if(claimType == ClaimTypeEnum.Dental) _context.TS837D.AddRangeAsync(ediTransactions.OfType<TS837D>());
           _context.SaveChanges();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to save edi data: " + e.Message +
                             (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }

    /// <summary>
    /// Asynchronous version of the method
    /// </summary>
    /// <param name="ediTransactions"></param>
    /// <param name="claimType"></param>
    /// <typeparam name="T"></typeparam>
    public async Task Save837Async<T>(List<T> ediTransactions, ClaimTypeEnum claimType) where T : EdiMessage
    {
        //await Task.Run(() => _logger.LogInformation($"Committing {ediTransactions.Count()} edi transactions..."));
        _logger.LogInformation($"Committing {ediTransactions.Count()} edi transactions...");
        try
        {
            if(claimType == ClaimTypeEnum.Professional) await _context.TS837P.AddRangeAsync(ediTransactions.OfType<TS837P>());
            if(claimType == ClaimTypeEnum.Institutional) await _context.TS837I.AddRangeAsync(ediTransactions.OfType<TS837I>());
            if(claimType == ClaimTypeEnum.Dental) await _context.TS837D.AddRangeAsync(ediTransactions.OfType<TS837D>());
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to save edi data: " + e.Message +
                             (e.InnerException != null ? e.InnerException.Message : "") + "\n" + e.StackTrace);
            return;
        }
    }
}