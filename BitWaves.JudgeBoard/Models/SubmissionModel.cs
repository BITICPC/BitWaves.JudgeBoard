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
        public LanguageModel Language { get; set; }

        /// <summary>
        /// Get or set the judge mode.
        /// </summary>
        [JsonProperty("judgeMode")]
        public ProblemJudgeMode JudgeMode { get; set; }

        /// <summary>
        /// Get or set the time limit, in milliseconds.
        /// </summary>
        [JsonProperty("timeLimit")]
        public int TimeLimit { get; set; }

        /// <summary>
        /// Get or set the memory limit, in megabytes.
        /// </summary>
        [JsonProperty("memoryLimit")]
        public int MemoryLimit { get; set; }

        /// <summary>
        /// Get or set the code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
