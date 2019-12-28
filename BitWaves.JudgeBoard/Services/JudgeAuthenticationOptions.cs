using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide options for judge node authentication service.
    /// </summary>
    public sealed class JudgeAuthenticationOptions
    {
        /// <summary>
        /// Initialize a new <see cref="JudgeAuthenticationOptions"/> instance.
        /// </summary>
        public JudgeAuthenticationOptions()
        {
            Expiration = TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// Get or set the time span after which a fresh authentication session will be expired.
        /// </summary>
        public TimeSpan Expiration { get; set; }

        /// <summary>
        /// Get or set the RSA key used to decrypt the challenge message.
        /// </summary>
        public RSA JudgePublicKey { get; set; }
    }

    /// <summary>
    /// Provide extension methods for <see cref="JudgeAuthenticationOptions"/>.
    /// </summary>
    public static class JudgeAuthenticationOptionsExtension
    {
        /// <summary>
        /// Load the <see cref="JudgeAuthenticationOptions.JudgePublicKey"/> from the public key part of the specified
        /// X509 certificate file.
        /// </summary>
        /// <param name="options">The <see cref="JudgeAuthenticationOptions"/> object.</param>
        /// <param name="file">Path to the certificate file.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="options"/> is null
        ///     or
        ///     <paramref name="file"/> is null.
        /// </exception>
        public static void LoadJudgePublicKeyFromCertificate(this JudgeAuthenticationOptions options, string file)
        {
            Contract.NotNull(options, nameof(options));
            Contract.NotNull(file, nameof(file));

            var cert = new X509Certificate2(file);
            var key = cert.GetRSAPublicKey();

            options.JudgePublicKey?.Dispose();
            options.JudgePublicKey = key;
        }
    }
}
