using System;

namespace BitWaves.JudgeBoard.Utils
{
    /// <summary>
    /// Provide an interface for buffer deque implementations.
    /// </summary>
    public interface IBufferDeque
    {
        /// <summary>
        /// Get the size of the buffer deque, i.e. the number of available bytes present in the buffer.
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Copy data from the given input buffer to the tail of the buffer.
        /// </summary>
        /// <param name="buffer">The input buffer.</param>
        void Push(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Copies data from the start of the buffer and removes them from the buffer.
        /// </summary>
        /// <param name="buffer">The output buffer.</param>
        /// <returns>Number of bytes actually transferred.</returns>
        int Pop(Span<byte> buffer);
    }
}
