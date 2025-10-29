using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        // Load configuration from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Get the connection string
        string connectionString = config.GetConnectionString("DefaultConnection");

        // Use the connection string in your DbContext
        using (var context = new EdiDbContext(connectionString))
        {
            // Example: Add a new claim
            var claim = new Claim
            {
                PatientName = "Jane Smith",
                ClaimNumber = "8370002",
                Amount = 456.78M
            };

            context.Claims.Add(claim);
            context.SaveChanges();

            // Read and display all claims
            var claims = context.Claims.ToList();
            foreach (var c in claims)
            {
                Console.WriteLine($"{c.Id}: {c.PatientName} - {c.ClaimNumber} - ${c.Amount}");
            }
        }
    }
}
