namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Represent the status of a judge node.
    /// </summary>
    public enum JudgeNodeStatus
    {
        /// <summary>
        /// The node is idle.
        /// </summary>
        Idle,

        /// <summary>
        /// The node is performing judge tasks.
        /// </summary>
        Judging
    }
}
