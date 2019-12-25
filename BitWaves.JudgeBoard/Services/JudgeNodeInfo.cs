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
            QueuedSubmissions = 0;
            IsBlocked = false;
            Performance = null;
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
        /// Get or set the number of queued submissions to the judge node.
        /// </summary>
        public int QueuedSubmissions { get; set; }

        /// <summary>
        /// Get or set whether to prevent sending submission information to this judge node.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Get or set the performance information about a judge node.
        /// </summary>
        public JudgeNodePerformanceInfo Performance { get; set; }
    }
}
