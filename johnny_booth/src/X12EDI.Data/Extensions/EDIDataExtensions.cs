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
    /// <summary>
    /// Provides extension methods for data-related operations and service registration.
    /// </summary>
    public static class EDIDataExtensions
    {
        #region Private Fields

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Registers the data layer services, including the DbContext and repositories,
        /// with the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configuration">The application configuration, used to retrieve the database connection string.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddX12EdiData(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IEdiDbContext, EdiDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("EdiDb")));

            services.AddScoped<IEdiRepository, EdiRepository>();

            return services;
        }

        /// <summary>
        /// Computes the SHA256 hash of a string.
        /// </summary>
        /// <param name="xml">The input string to hash.</param>
        /// <returns>A hexadecimal string representation of the SHA256 hash.</returns>
        public static string ComputeSha256(string xml)
        {
            var bytes = Encoding.UTF8.GetBytes(xml);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// Serializes an <see cref="EdiMessage"/> to a JSON string.
        /// </summary>
        /// <param name="message">The <see cref="EdiMessage"/> to serialize.</param>
        /// <returns>A JSON string representation of the message.</returns>
        public static string ToJson(this EdiMessage message)
        {
            var options = _jsonOptions;

            return JsonSerializer.Serialize(message, message.GetType(), options);
        }

        /// <summary>
        /// Serializes an <see cref="EdiMessage"/> to an XML string.
        /// </summary>
        /// <param name="message">The <see cref="EdiMessage"/> to serialize.</param>
        /// <returns>An XML string representation of the message.</returns>
        public static string ToXml(this EdiMessage message)
        {
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(message.GetType());
            serializer.Serialize(stringWriter, message);
            return stringWriter.ToString();
        }

        #endregion Public Methods
    }
}