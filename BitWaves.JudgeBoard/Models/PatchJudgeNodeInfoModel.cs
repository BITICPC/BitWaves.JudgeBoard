using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide a model type for patching judge node information.
    /// </summary>
    public sealed class PatchJudgeNodeInfoModel
    {
        /// <summary>
        /// Get or set the address of the judge node.
        /// </summary>
        [JsonIgnore]
        public IPAddress Address { get; set; }

        /// <summary>
        /// Get or set the timestamp of the heartbeat packet.
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Get or set the timestamp of the last heartbeat packet sent by the judge node.
        /// </summary>
        [JsonIgnore]
        public DateTime LastHeartBeat { get; set; }

        /// <summary>
        /// Get or set the timestamp of the last packet sent by the judge node.
        /// </summary>
        [JsonIgnore]
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Get or set the CPU usage of the judge node, in percentage form.
        /// </summary>
        [JsonProperty("cpuUsage")]
        [Range(0.0, 1.0)]
        public double CpuUsage { get; set; }

        /// <summary>
        /// Get or set the number of cores installed on the judge node.
        /// </summary>
        [JsonProperty("cores")]
        [Range(1, int.MaxValue)]
        public int Cores { get; set; }

        /// <summary>
        /// Get or set the total amount of physical memory installed on the judge node, in megabytes.
        /// </summary>
        [JsonProperty("totalPhysicalMemory")]
        [Range(1, long.MaxValue)]
        public long TotalPhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the total amount of free memory on the judge node, in megabytes.
        /// </summary>
        [JsonProperty("freePhysicalMemory")]
        [Range(0, long.MaxValue)]
        public long FreePhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the size of the swap file, in megabytes.
        /// </summary>
        [JsonProperty("swapFileSize")]
        [Range(0, long.MaxValue)]
        public long SwapFileSize { get; set; }
    }
}
