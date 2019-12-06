using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide data model for submission judge results.
    /// </summary>
    public sealed class SubmissionJudgeResultModel
    {
        /// <summary>
        /// Get or set the overall verdict.
        /// </summary>
        [JsonProperty("verdict")]
        public Verdict Verdict { get; set; }

        /// <summary>
        /// Get or set the time usage, in milliseconds.
        /// </summary>
        [JsonProperty("time")]
        public int Time { get; set; }

        /// <summary>
        /// Get or set the memory usage, in milliseconds.
        /// </summary>
        [JsonProperty("memory")]
        public int Memory { get; set; }

        /// <summary>
        /// Get or set the judge results of all test cases.
        /// </summary>
        [JsonProperty("testCases")]
        public TestCaseJudgeResultModel[] TestCases { get; set; }
    }
}
