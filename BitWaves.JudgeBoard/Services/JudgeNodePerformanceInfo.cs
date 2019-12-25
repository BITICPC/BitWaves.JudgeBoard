namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Performance information about a judge node.
    /// </summary>
    public sealed class JudgeNodePerformanceInfo
    {
        /// <summary>
        /// Get or set the overall CPU usage ratio of the judge node.
        /// </summary>
        public double CpuUsage { get; set; }

        /// <summary>
        /// Get or set the number of cores installed on the judge node.
        /// </summary>
        public int Cores { get; set; }

        /// <summary>
        /// Get or set the total physical memory installed on the judge node, measured in megabytes.
        /// </summary>
        public long TotalPhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the free physical memory on the judge node, measured in megabytes.
        /// </summary>
        public long FreePhysicalMemory { get; set; }

        /// <summary>
        /// Get or set the swap file size, measured in megabytes.
        /// </summary>
        public long SwapFileSize { get; set; }

        /// <summary>
        /// Get or set the cached swap space size, measured in megabytes.
        /// </summary>
        public long CachedSwapSpace { get; set; }
    }
}
