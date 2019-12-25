using Newtonsoft.Json;

namespace BitWaves.JudgeBoard.Models
{
    /// <summary>
    /// Provide a data model for judge node performance information.
    /// </summary>
    public sealed class JudgeNodePerformanceInfoModel
    {
        /// <summary>
        /// Get or set the overall CPU usage ratio of the judge node.
        /// </summary>
        [JsonProperty("cpuUsage")]
        public double CpuUsage { get; set; }

        /// <summary>
        /// Get or set the number of cores installed on the judge node.
        /// </summary>
        [JsonProperty("cores")]
        public int Cores { get; set; }

        /// <summary>
        /// Get or set the total physical memory installed on the judge node, measured in megabytes.
        /// </summary>
        [JsonProperty("totalPhysicalMemory")]
        public long TotalPhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the free physical memory on the judge node, measured in megabytes.
        /// </summary>
        [JsonProperty("freePhysicalMemory")]
        public long FreePhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the swap file size, measured in megabytes.
        /// </summary>
        [JsonProperty("swapFileSize")]
        public long SwapFileSize { get; set; }

        /// <summary>
        /// Get or set the cached swap space size, measured in megabytes.
        /// </summary>
        [JsonProperty("cachedSwapSpace")]
        public long CachedSwapSpace { get; set; }
    }
}
