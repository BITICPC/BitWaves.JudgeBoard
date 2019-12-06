using System;
using System.Threading.Tasks;

namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide abstractions to manage all connected judge nodes.
    /// </summary>
    public interface IJudgeNodeManager
    {
        /// <summary>
        /// Update the existing judge node information with the given one.
        /// </summary>
        /// <remarks>
        /// If the specified judge node information already exist in the judge node manager, then the
        /// <see cref="JudgeNodeInfo.Status"/> property of <paramref name="info"/> will be ignored.
        /// </remarks>
        /// <param name="info">
        ///     The newest judge node information. If the judge node info does not exist in the backing store, a new
        ///     entry in the store will be created.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="info"/> is null.</exception>
        Task UpdateAsync(JudgeNodeInfo info);

        /// <summary>
        /// Get all online judge nodes.
        /// </summary>
        /// <returns>All online judge nodes.</returns>
        Task<JudgeNodeInfo[]> GetAllAsync();
    }
}
