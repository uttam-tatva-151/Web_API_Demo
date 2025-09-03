using Common.ConstantHandler;
using Common.DataEncryption;
using Data;
using Microsoft.EntityFrameworkCore;
namespace Web_API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring database-related services.
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Configures the application's database services, including PostgreSQL connection and DbContext.
        /// </summary>
        /// <param name="services">The IServiceCollection instance to add database services to.</param>
        /// <param name="configuration">The IConfiguration instance to retrieve connection string settings from.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        /// <remarks>
        /// This method performs the following:
        /// - Retrieves the default database connection string from the configuration.
        /// - Registers PostgreSQL composite types using NpgsqlDataSourceBuilder.
        /// - Configures the application's DbContext to use PostgreSQL.
        /// Ensure that the connection string is properly defined in the appsettings.json file.
        /// </remarks>
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            AESEncryptionHelper aesEncryptionHelper = new();

            string? encrypttedDatabaseConnectionString = configuration.GetConnectionString(Constants.DATABASE_DEFAULT_CONNECTION);
            string? keyBase64 = configuration[Constants.AES_ENCRYPTION_KEY];
            string? ivBase64 = configuration[Constants.AES_ENCRYPTION_IV];

            if (string.IsNullOrWhiteSpace(encrypttedDatabaseConnectionString))
                throw new InvalidOperationException(Messages.ERROR_MISSING_DB_CONNECTION);
            if (string.IsNullOrWhiteSpace(keyBase64) || string.IsNullOrWhiteSpace(ivBase64))
                throw new InvalidOperationException(Messages.ERROR_MISSING_ENCRYPTION_KEYS);

            string databaseConnectionString = AESEncryptionHelper.DecryptString(encrypttedDatabaseConnectionString, keyBase64, ivBase64);
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(databaseConnectionString));
            return services;
        }
    }
}

