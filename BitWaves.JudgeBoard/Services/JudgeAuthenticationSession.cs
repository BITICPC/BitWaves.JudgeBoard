using System;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide information about a judge node authentication session.
    /// </summary>
    public sealed class JudgeAuthenticationSession
    {
        /// <summary>
        /// Initialize a new <see cref="JudgeAuthenticationSession"/> instance.
        /// </summary>
        /// <param name="sessionId">ID of the session.</param>
        /// <param name="expireTime">Expire time of this session.</param>
        /// <param name="challenge">The challenge data.</param>
        /// <param name="encryptedChallenge">The encrypted challenge message.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="challenge"/> is null
        ///     or
        ///     <paramref name="encryptedChallenge"/> is null.
        /// </exception>
        public JudgeAuthenticationSession(Guid sessionId, DateTime expireTime,
                                          byte[] challenge, byte[] encryptedChallenge)
        {
            Contract.NotNull(challenge, nameof(challenge));
            Contract.NotNull(encryptedChallenge, nameof(encryptedChallenge));

            SessionId = sessionId;
            ExpireTime = expireTime;
            Challenge = challenge;
            EncryptedChallenge = encryptedChallenge;
        }

        /// <summary>
        /// Get the ID of the session.
        /// </summary>
        public Guid SessionId { get; }

        /// <summary>
        /// Get the expiration time of the session.
        /// </summary>
        public DateTime ExpireTime { get; }

        /// <summary>
        /// Get the raw data of the challenge.
        /// </summary>
        public byte[] Challenge { get; }

        /// <summary>
        /// Get the encrypted challenge message.
        /// </summary>
        public byte[] EncryptedChallenge { get; }

        /// <summary>
        /// Determine whether the session has expired.
        /// </summary>
        public bool IsExpired => DateTime.UtcNow > ExpireTime;
    }
}
