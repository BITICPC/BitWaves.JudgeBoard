using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide data model for languages.
    /// </summary>
    public sealed class LanguageModel
    {
        /// <summary>
        /// Get or set the identifier of the language.
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// Get or set the dialect of the language.
        /// </summary>
        [JsonProperty("dialect")]
        public string Dialect { get; set; }

        /// <summary>
        /// Get or set the version of the language.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
