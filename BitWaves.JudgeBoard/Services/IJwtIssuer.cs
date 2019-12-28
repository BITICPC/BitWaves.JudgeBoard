using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide interface for issuing JWTss
    /// </summary>
    public interface IJwtIssuer
    {
        /// <summary>
        /// Issue a new JWT.
        /// </summary>
        /// <param name="name">Name of the identity.</param>
        /// <returns>The issued JWT.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        Task<string> IssueAsync(string name);
    }

    /// <summary>
    /// Provide extension methods for <see cref="IJwtIssuer"/>.
    /// </summary>
    public static class JwtIssuerExtensions
    {
        /// <summary>
        /// Add the default implementation of <see cref="IJwtIssuer"/> to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="options">Callback to manipulate options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
        public static IServiceCollection AddDefaultJwtIssuer(
            this IServiceCollection services,
            Action<JwtIssuerOptions> options = null)
        {
            Contract.NotNull(services, nameof(services));

            var opt = new JwtIssuerOptions();
            options?.Invoke(opt);

            services.AddSingleton(opt);
            services.AddSingleton<IJwtIssuer, DefaultJwtIssuer>();

            return services;
        }
    }
}
