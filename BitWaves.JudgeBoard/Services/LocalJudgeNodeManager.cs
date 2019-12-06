using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide an implementation of <see cref="IJudgeNodeManager"/> that uses a locally maintained
    /// <see cref="ConcurrentDictionary{TKey,TValue}"/> as the backing store.
    /// </summary>
    internal sealed class LocalJudgeNodeManager : IJudgeNodeManager
    {
        private readonly JudgeNodeManagerOptions _options;
        private readonly ConcurrentDictionary<IPAddress, JudgeNodeInfo> _store;

        /// <summary>
        /// Initialize a new <see cref="LocalJudgeNodeManager"/> instance.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
        public LocalJudgeNodeManager(JudgeNodeManagerOptions options)
        {
            Contract.NotNull(options, nameof(options));

            _options = options;
            _store = new ConcurrentDictionary<IPAddress, JudgeNodeInfo>();
        }

        /// <summary>
        /// Determines whether the given integer is a non-negative exponent of 2, i.e. <paramref name="number"/> is
        /// 1, 2, 4, 8, 16, 32, ...
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>Whether the given integer is a power of 2.</returns>
        private static bool IsPowerOfTwo(int number)
        {
            if (number <= 0)
            {
                return false;
            }

            var unsigned = (uint) number;
            return (unsigned & (unsigned - 1)) == 0;
        }

        /// <inheritdoc />
        public Task UpdateAsync(JudgeNodeInfo info)
        {
            Contract.NotNull(info, nameof(info));

            _store.AddOrUpdate(info.Address, info, (_, old) =>
            {
                info.Status = old.Status;
                return info;
            });

            // If the number of elements in the backing store hit an exponent of 2 then invoke RemoveExpired method
            // to sweep out all expired judge nodes.
            if (IsPowerOfTwo(_store.Count))
            {
                RemoveExpired();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Perform a full scan over the backing store and removes those <see cref="JudgeNodeInfo"/> objects which have
        /// expired already.
        /// </summary>
        private void RemoveExpired()
        {
            var timestamp = DateTime.UtcNow;
            var expiredKeys = new List<IPAddress>();

            foreach (var (address, info) in _store)
            {
                if (info.LastSeen.AddMinutes(_options.ExpirationDuration) < timestamp)
                {
                    // The current entry has expired.
                    expiredKeys.Add(address);
                }
            }

            foreach (var key in expiredKeys)
            {
                _store.TryRemove(key, out _);
            }
        }

        /// <inheritdoc />
        public Task<JudgeNodeInfo[]> GetAllAsync()
        {
            RemoveExpired();
            return Task.FromResult(_store.Values.ToArray());
        }
    }

    /// <summary>
    /// Provide extension methods to <see cref="LocalJudgeNodeManager"/> class.
    /// </summary>
    internal static class LocalJudgeNodeManagerExtensions
    {
        /// <summary>
        /// Add <see cref="LocalJudgeNodeManager"/> as the desired implementation of <see cref="IJudgeNodeManager"/>
        /// into the given service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="optionCallback">The callback for configuring options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is null.</exception>
        public static IServiceCollection AddLocalJudgeNodeManager(this IServiceCollection services,
                                                                  Action<JudgeNodeManagerOptions> optionCallback = null)
        {
            Contract.NotNull(services, nameof(services));

            var options = new JudgeNodeManagerOptions();
            optionCallback?.Invoke(options);

            return services.AddSingleton(options)
                           .AddSingleton<IJudgeNodeManager, LocalJudgeNodeManager>();
        }
    }
}
