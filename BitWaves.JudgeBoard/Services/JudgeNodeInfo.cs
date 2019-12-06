using System;
using System.Net;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide information about a judge node.
    /// </summary>
    public sealed class JudgeNodeInfo
    {
        /// <summary>
        /// Initialize a new <see cref="JudgeNodeInfo"/> instance.
        /// </summary>
        /// <param name="address">The address of the judge node.</param>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> is null.</exception>
        public JudgeNodeInfo(IPAddress address)
        {
            Contract.NotNull(address, nameof(address));

            Address = address;
            LastHeartBeat = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }

        /// <summary>
        /// Get the address of the node.
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        /// Get or set the timestamp of the last heart beat packet sent by the node.
        /// </summary>
        public DateTime LastHeartBeat { get; set; }

        /// <summary>
        /// Get or set the timestamp of the last request sent by the node.
        /// </summary>
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Get or set the status of the judge.
        /// </summary>
        public JudgeNodeStatus Status { get; set; }

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
    }
}
