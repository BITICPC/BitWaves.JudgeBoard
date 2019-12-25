using System;
using System.Net;
using System.Threading.Tasks;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide abstractions to manage all connected judge nodes.
    /// </summary>
    public interface IJudgeNodeManager
    {
        /// <summary>
        /// Update the performance information of the specified judge node. This operation will also update the
        /// <see cref="JudgeNodeInfo.LastSeen"/> property and the <see cref="JudgeNodeInfo.LastHeartBeat"/> property.
        /// If the specified judge node does not exist yet, this method will add a new judge node record in the manager.
        /// </summary>
        /// <param name="address">The address of the judge node.</param>
        /// <param name="performanceInfo">The performance information.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="address"/> is null
        ///     or
        ///     <paramref name="performanceInfo"/> is null.
        /// </exception>
        Task UpdatePerformanceAsync(IPAddress address, JudgeNodePerformanceInfo performanceInfo);

        /// <summary>
        /// Update the last seen timestamp of the specified judge node. If the specified judge node does not exist yet,
        /// this method will add a new judge node record in the manager.
        /// </summary>
        /// <param name="address">Address of the judge node.</param>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> is null.</exception>
        Task UpdateLastSeenAsync(IPAddress address);

        /// <summary>
        /// Set the blocked state of the specified judge.
        /// </summary>
        /// <param name="address">Address of the judge node.</param>
        /// <param name="blocked">Whether the judge node is blocked.</param>
        /// <returns>Whether the update operation succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> is null.</exception>
        Task<bool> SetBlockedAsync(IPAddress address, bool blocked = true);

        /// <summary>
        /// Get the blocked state of the specified judge.
        /// </summary>
        /// <param name="address">Address of the judge node.</param>
        /// <returns>The blocked state of the specified judge.</returns>
        Task<bool> IsBlockedAsync(IPAddress address);

        /// <summary>
        /// Increase the judge queue length of the specified judge node.
        /// </summary>
        /// <param name="address">Address of the judge node.</param>
        /// <param name="increment">Increment of the judge queue length.</param>
        /// <returns>Whether the update operation succeeded.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> is null.</exception>
        Task<bool> IncreaseQueue(IPAddress address, int increment = 1);

        /// <summary>
        /// Get all online judge nodes.
        /// </summary>
        /// <returns>All online judge nodes.</returns>
        Task<JudgeNodeInfo[]> GetAllAsync();
    }
}
