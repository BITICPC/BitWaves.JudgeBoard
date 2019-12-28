using System;
using Microsoft.IdentityModel.Tokens;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide options for JWT issuers.
    /// </summary>
    public sealed class JwtIssuerOptions
    {
        /// <summary>
        /// Initialize a new <see cref="JwtIssuerOptions"/> instance.
        /// </summary>
        public JwtIssuerOptions()
        {
            Expiration = null;
            SigningKey = null;
        }

        /// <summary>
        /// Get or set the expire duration of a JWT.
        /// </summary>
        public TimeSpan? Expiration { get; set; }

        /// <summary>
        /// Get or set the RSA private key to sign the JWT.
        /// </summary>
        public SecurityKey SigningKey { get; set; }
    }
}
