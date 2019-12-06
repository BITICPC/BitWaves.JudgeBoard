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
        /// Get or set the status of the judge node.
        /// </summary>
        [JsonProperty("status")]
        public JudgeNodeStatus Status { get; set; }

        /// <summary>
        /// Get or set the CPU usage of the judge node, in percentage form.
        /// </summary>
        [JsonProperty("cpuUsage")]
        public double CpuUsage { get; set; }

        /// <summary>
        /// Get or set the number of cores installed on the judge node.
        /// </summary>
        [JsonProperty("cores")]
        public int Cores { get; set; }

        /// <summary>
        /// Get or set the total physical memory installed on the judge node, in megabytes.
        /// </summary>
        [JsonProperty("totalPhysicalMemory")]
        public long TotalPhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the free physical memory on the judge node, in megabytes.
        /// </summary>
        [JsonProperty("freePhysicalMemory")]
        public long FreePhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the size of the swap file on the judge node, in megabytes.
        /// </summary>
        [JsonProperty("swapFileSize")]
        public long SwapFileSize { get; set; }
    }
}
