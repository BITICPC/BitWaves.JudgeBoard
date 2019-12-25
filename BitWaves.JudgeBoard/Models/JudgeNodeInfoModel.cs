using System;
using BitWaves.JudgeBoard.Services;
using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide definition for judge node information model.
    /// </summary>
    public sealed class JudgeNodeInfoModel
    {
        /// <summary>
        /// Get or set the address of the judge node.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Get or set the timestamp of the last heartbeat packet sent by the judge node.
        /// </summary>
        [JsonProperty("lastHeartBeat")]
        public DateTime LastHeartBeat { get; set; }

        /// <summary>
        /// Get or set the timestamp of the last request sent by the judge node.
        /// </summary>
        [JsonProperty("lastSeen")]
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Get or set the number of queued submissions to the judge node.
        /// </summary>
        [JsonProperty("queuedSubmissions")]
        public int QueuedSubmissions { get; set; }

        /// <summary>
        /// Get or set whether to prevent sending submission information to this judge node.
        /// </summary>
        [JsonProperty("isBlocked")]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Get or set the performance model.
        /// </summary>
        [JsonProperty("performance")]
        public JudgeNodePerformanceInfoModel Performance { get; set; }
    }
}
