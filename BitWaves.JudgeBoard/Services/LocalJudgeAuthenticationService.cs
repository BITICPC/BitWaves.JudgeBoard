using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide an implementation of <see cref="IJudgeAuthenticationService"/> that stores authentication sessions in
    /// an underlying hash table.
    /// </summary>
    internal sealed class LocalJudgeAuthenticationService : IJudgeAuthenticationService
    {
        /// <summary>
        /// Size of the challenge message, in bytes.
        /// </summary>
        private const int ChallengeSize = 16;

        private readonly JudgeAuthenticationOptions _options;
        private readonly ConcurrentDictionary<Guid, JudgeAuthenticationSession> _sessions;
        private readonly Random _rng;

        /// <summary>
        /// Initialize a new <see cref="LocalJudgeAuthenticationService"/> instance.
        /// </summary>
        /// <param name="options">Service options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
        public LocalJudgeAuthenticationService(JudgeAuthenticationOptions options)
        {
            Contract.NotNull(options, nameof(options));

            _options = options;
            _sessions = new ConcurrentDictionary<Guid, JudgeAuthenticationSession>();
            _rng = new Random();
        }

        /// <summary>
        /// Generate a new challenge message.
        /// </summary>
        /// <returns>A new generated challenge message.</returns>
        private byte[] GenerateChallenge()
        {
            var challenge = new byte[ChallengeSize];
            _rng.NextBytes(challenge);
            return challenge;
        }

        /// <summary>
        /// Remove all expired authentication sessions.
        /// </summary>
        private void RemoveExpired()
        {
            var currentTime = DateTime.UtcNow;
            foreach (var (id, session) in _sessions)
            {
                if (currentTime > session.ExpireTime)
                {
                    _sessions.TryRemove(id, out _);
                }
            }
        }

        /// <summary>
        /// Remove all expired authentication sessions if the number of registered sessions is some exponent of 2.
        /// </summary>
        private void RemoveExpiredOnNecessary()
        {
            var count = (uint) _sessions.Count;
            if ((count & (count - 1)) == 0)
            {
                RemoveExpired();
            }
        }

        /// <inheritdoc />
        public Task<JudgeAuthenticationSession> CreateSessionAsync()
        {
            var sessionId = Guid.NewGuid();
            var expireTime = DateTime.UtcNow + _options.Expiration;
            var challenge = GenerateChallenge();
            var encryptedChallenge = _options.JudgePublicKey.Encrypt(challenge, RSAEncryptionPadding.Pkcs1);

            var session = new JudgeAuthenticationSession(sessionId, expireTime, challenge, encryptedChallenge);
            _sessions.TryAdd(sessionId, session);

            RemoveExpiredOnNecessary();

            return Task.FromResult(session);
        }

        /// <inheritdoc />
        public Task<bool> ChallengeAsync(Guid sessionId, byte[] response)
        {
            Contract.NotNull(response, nameof(response));

            if (!_sessions.TryGetValue(sessionId, out var session))
            {
                return Task.FromResult(false);
            }

            if (session.IsExpired)
            {
                _sessions.TryRemove(sessionId, out _);
                return Task.FromResult(false);
            }

            var ret = (ReadOnlySpan<byte>) session.Challenge == (ReadOnlySpan<byte>) response;

            // Remove the session from registered sessions
            _sessions.TryRemove(sessionId, out _);

            return Task.FromResult(ret);
        }
    }
}
