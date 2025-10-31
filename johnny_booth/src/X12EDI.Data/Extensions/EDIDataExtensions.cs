using EdiFabric.Core.Model.Edi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using X12EDI.Abstractions.Repositories;
using X12EDI.Data.DBContext;
using X12EDI.Data.Repositories;

namespace X12EDI.Data.Extensions
{
    public static class EDIDataExtensions
    {
        #region Public Methods

        public static IServiceCollection AddX12EdiData(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<EdiDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EdiDb")));

            services.AddScoped<IEdiRepository, EdiRepository>();

            return services;
        }

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static string ToJson(this EdiMessage message)
        {
            var options = _jsonOptions;

            return JsonSerializer.Serialize(message, message.GetType(), options);
        }

        public static string ToXml(this EdiMessage message)
        {
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(message.GetType());
            serializer.Serialize(stringWriter, message);
            return stringWriter.ToString();
        }

    public static string ComputeSha256(string xml)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(xml);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hash); // .NET 5+; use BitConverter if older
    }

    #endregion Public Methods
}
}