using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide an implementation of <see cref="IJudgeNodeManager"/> that uses a locally maintained
    /// <see cref="Dictionary{TKey,TValue}"/> as the backing store.
    /// </summary>
    internal sealed class LocalJudgeNodeManager : IJudgeNodeManager
    {
        private readonly JudgeNodeManagerOptions _options;
        private readonly Dictionary<IPAddress, JudgeNodeInfo> _store;

        /// <summary>
        /// Initialize a new <see cref="LocalJudgeNodeManager"/> instance.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
        public LocalJudgeNodeManager(JudgeNodeManagerOptions options)
        {
            Contract.NotNull(options, nameof(options));

            _options = options;
            _store = new Dictionary<IPAddress, JudgeNodeInfo>();
        }

        /// <summary>
        /// Lock the store and invokes the given callback on the internal store.
        /// </summary>
        /// <param name="callback">The callback to be invoked with the internal store.</param>
        /// <typeparam name="T">The return type of the callback.</typeparam>
        /// <returns>Whatever the given callback returns.</returns>
        private T WithStore<T>(Func<Dictionary<IPAddress, JudgeNodeInfo>, T> callback)
        {
            Contract.NotNull(callback, nameof(callback));

            lock (_store)
            {
                return callback(_store);
            }
        }

        /// <summary>
        /// Lock the store and invokes the given callback on the internal store.
        /// </summary>
        /// <param name="callback">The callback to be invoked with the internal store.</param>
        private void WithStore(Action<Dictionary<IPAddress, JudgeNodeInfo>> callback)
        {
            Contract.NotNull(callback, nameof(callback));

            lock (_store)
            {
                callback(_store);
            }
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

        /// <summary>
        /// Perform a full scan over the backing store and removes those <see cref="JudgeNodeInfo"/> objects which have
        /// expired already. It is the caller's responsibility to lock the internal store before calling this method.
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
                _store.Remove(key);
            }
        }

        /// <summary>
        /// Remove expired judge node record if the number of judge nodes registered is a power of 2. It is the caller's
        /// responsibility to lock the internal store before calling this method.
        /// </summary>
        private void RemoveExpiredOnNecessary()
        {
            if (IsPowerOfTwo(_store.Count))
            {
                RemoveExpired();
            }
        }

        /// <inheritdoc />
        public Task UpdatePerformanceAsync(IPAddress address, JudgeNodePerformanceInfo performanceInfo)
        {
            Contract.NotNull(address, nameof(address));
            Contract.NotNull(performanceInfo, nameof(performanceInfo));

            WithStore(store =>
            {
                if (store.TryGetValue(address, out var info))
                {
                    info.Performance = performanceInfo;
                    info.LastHeartBeat = DateTime.UtcNow;
                    info.LastSeen = DateTime.UtcNow;
                }
                else
                {
                    var newInfo = new JudgeNodeInfo(address)
                    {
                        Performance = performanceInfo,
                        LastHeartBeat = DateTime.UtcNow,
                        LastSeen = DateTime.UtcNow
                    };
                    store.Add(address, newInfo);
                    RemoveExpiredOnNecessary();
                }
            });

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task UpdateLastSeenAsync(IPAddress address)
        {
            Contract.NotNull(address, nameof(address));

            WithStore(store =>
            {
                if (store.TryGetValue(address, out var info))
                {
                    info.LastSeen = DateTime.UtcNow;
                }
                else
                {
                    var newInfo = new JudgeNodeInfo(address)
                    {
                        LastSeen = DateTime.UtcNow
                    };
                    store.Add(address, newInfo);
                    RemoveExpiredOnNecessary();
                }
            });

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<bool> SetBlockedAsync(IPAddress address, bool blocked = true)
        {
            Contract.NotNull(address, nameof(address));

            var ret = WithStore(store =>
            {
                if (!store.TryGetValue(address, out var info))
                {
                    return false;
                }

                info.IsBlocked = blocked;
                return true;
            });

            return Task.FromResult(ret);
        }

        /// <inheritdoc />
        public Task<bool> IsBlockedAsync(IPAddress address)
        {
            Contract.NotNull(address, nameof(address));

            var ret = WithStore(store =>
            {
                if (!store.TryGetValue(address, out var info))
                {
                    // If the judge node cannot be found, then return true to ensure that submission information will
                    // not leak by accident.
                    return true;
                }

                return info.IsBlocked;
            });

            return Task.FromResult(ret);
        }

        /// <inheritdoc />
        public Task<bool> IncreaseQueue(IPAddress address, int increment = 1)
        {
            Contract.NotNull(address, nameof(address));

            var ret = WithStore(store =>
            {
                if (!store.TryGetValue(address, out var info))
                {
                    return false;
                }

                info.QueuedSubmissions += increment;
                return true;
            });

            return Task.FromResult(ret);
        }

        /// <inheritdoc />
        public Task<JudgeNodeInfo[]> GetAllAsync()
        {
            var ret = WithStore(store =>
            {
                RemoveExpired();
                return store.Values.ToArray();
            });
            return Task.FromResult(ret);
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
