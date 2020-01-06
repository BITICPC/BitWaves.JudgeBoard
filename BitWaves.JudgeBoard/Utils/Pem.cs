using System;
using System.IO;
using System.Security.Cryptography;
using PemUtils;

namespace BitWaves.JudgeBoard.Utils
{
    /// <summary>
    /// Provide utilities for dealing with PEM files.
    /// </summary>
    public static class Pem
    {
        /// <summary>
        /// Read a RSA key from the given PEM file.
        /// </summary>
        /// <param name="pemFile">Path to the PEM file.</param>
        /// <returns>The RSA key read from the PEM file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pemFile"/> is null.</exception>
        public static RSA ReadRsaKey(string pemFile)
        {
            Contract.NotNull(pemFile, nameof(pemFile));

            RSAParameters rsaParams;
            using (var pemReader = new PemReader(File.OpenRead(pemFile)))
            {
                rsaParams = pemReader.ReadRsaKey();
            }

            return RSA.Create(rsaParams);
        }
    }
}
