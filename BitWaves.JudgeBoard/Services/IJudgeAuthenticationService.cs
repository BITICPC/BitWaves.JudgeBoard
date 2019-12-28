using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide interface for judge node authentication.
    /// </summary>
    public interface IJudgeAuthenticationService
    {
        /// <summary>
        /// Create a new judge node authentication session.
        /// </summary>
        /// <returns>The created judge node authentication session.</returns>
        Task<JudgeAuthenticationSession> CreateSessionAsync();

        /// <summary>
        /// Challenge the specified authentication session.
        /// </summary>
        /// <param name="sessionId">ID of the session.</param>
        /// <param name="response">Judge node response against the session.</param>
        /// <returns>Whether the challenge is successful.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="response"/> is null.</exception>
        Task<bool> ChallengeAsync(Guid sessionId, byte[] response);
    }

    /// <summary>
    /// Provide extension methods for <see cref="LocalJudgeAuthenticationService"/>.
    /// </summary>
    public static class IJudgeAuthenticationServiceExtensions
    {
        /// <summary>
        /// Add <see cref="LocalJudgeAuthenticationService"/> to the given service collection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="options">A callback to manipulate options.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="services"/> is null
        ///     or
        ///     <paramref name="options"/> is null.
        /// </exception>
        public static IServiceCollection AddLocalJudgeAuthenticationService(
            this IServiceCollection services,
            Action<JudgeAuthenticationOptions> options)
        {
            Contract.NotNull(services, nameof(services));
            Contract.NotNull(options, nameof(options));

            var opt = new JudgeAuthenticationOptions();
            options(opt);

            services.AddSingleton(opt);
            services.AddSingleton<IJudgeAuthenticationService, LocalJudgeAuthenticationService>();

            return services;
        }
    }
}
