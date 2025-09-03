using Services.Interfaces;

namespace Web_API.Extensions
{
    /// <summary>
    /// Provides extension methods for registering application services.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers application services used in the business logic layer.
        /// </summary>
        /// <param name="services">The IServiceCollection instance to add application services to.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        /// <remarks>
        /// This method registers the following services:
        /// - IErrorLogService with ErrorLogService
        /// - IUserService with UserService
        /// - IJWTService with JWTService
        /// - ILoginService with LoginService
        /// Ensure that these services are properly implemented and follow the application's business logic patterns.
        /// </remarks>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                    .FromAssemblyOf<IManufacturerService>()
                    // .AddClasses(classes => classes.Where(type => type.Name.EndsWith("JWTService")))
                    // .AsImplementedInterfaces()
                    // .WithSingletonLifetime()
                    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                );
            return services;
        }
    }
}

