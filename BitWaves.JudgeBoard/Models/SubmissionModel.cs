using BitWaves.Data.Entities;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide data model for submissions.
    /// </summary>
    public sealed class SubmissionModel
    {
        /// <summary>
        /// Get or set the ID of the submission.
        /// </summary>
        [JsonProperty("id")]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Get or set the ID of the archive.
        /// </summary>
        [JsonProperty("archiveId")]
        public ObjectId ArchiveId { get; set; }

        /// <summary>
        /// Get or set the language of the submission.
        /// </summary>
        [JsonProperty("language")]
        public LanguageModel LanguageTriple { get; set; }

        /// <summary>
        /// Get or set the code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
