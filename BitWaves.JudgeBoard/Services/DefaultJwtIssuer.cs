using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide a default implementation of <see cref="IJwtIssuer"/>.
    /// </summary>
    internal sealed class DefaultJwtIssuer : IJwtIssuer
    {
        private readonly JwtIssuerOptions _options;

        /// <summary>
        /// Initialize a new <see cref="DefaultJwtIssuer"/> instance.
        /// </summary>
        /// <param name="options">The issuer options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
        public DefaultJwtIssuer(JwtIssuerOptions options)
        {
            Contract.NotNull(options, nameof(options));

            _options = options;
        }

        /// <inheritdoc />
        public Task<string> IssueAsync(string name)
        {
            Contract.NotNull(name, nameof(name));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, name),
                }),
                Expires = DateTime.UtcNow + _options.Expiration,
                SigningCredentials = new SigningCredentials(_options.SigningKey, SecurityAlgorithms.RsaSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Task.FromResult(jwt);
        }
    }
}
