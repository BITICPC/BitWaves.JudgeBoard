namespace BitWaves.JudgeBoard.Services
{
    /// <summary>
    /// Provide options for <see cref="IJudgeNodeManager"/> services.
    /// </summary>
    public sealed class JudgeNodeManagerOptions
    {
        private const int DefaultExpirationDuration = 5;

        /// <summary>
        /// Initialize a new <see cref="JudgeNodeManagerOptions"/> instance.s
        /// </summary>
        public JudgeNodeManagerOptions()
        {
            ExpirationDuration = DefaultExpirationDuration;
        }

        /// <summary>
        /// Get or set the expiration duration of judge nodes, measured in minutes.
        /// </summary>
        public int ExpirationDuration { get; set; }
    }
}
