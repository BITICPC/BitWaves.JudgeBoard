using BitWaves.Data.Entities;
using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide data model for test case judge result.
    /// </summary>
    public sealed class TestCaseJudgeResultModel
    {
        /// <summary>
        /// Get or set the verdict.
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
        /// Get or set the exit code.
        /// </summary>
        [JsonProperty("exitCode")]
        public int ExitCode { get; set; }

        /// <summary>
        /// Get or set the judge's comment.
        /// </summary>
        [JsonProperty("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Get or set the input data view.
        /// </summary>
        [JsonProperty("inputView")]
        public string InputView { get; set; }

        /// <summary>
        /// Get or set the answer data view.
        /// </summary>
        [JsonProperty("answerView")]
        public string AnswerView { get; set; }

        /// <summary>
        /// Get or set the output data view.
        /// </summary>
        [JsonProperty("outputView")]
        public string OutputView { get; set; }
    }
}
