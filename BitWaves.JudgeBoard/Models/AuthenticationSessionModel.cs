using System;
using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide a data model for authentication sessions.
    /// </summary>
    public sealed class AuthenticationSessionModel
    {
        /// <summary>
        /// Get or set the ID of the authentication session.
        /// </summary>
        [JsonProperty("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Get or set the challenge message of the authentication session.
        /// </summary>
        [JsonProperty("challenge")]
        public byte[] Challenge { get; set; }
    }
}
