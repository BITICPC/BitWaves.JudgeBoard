using System;
using System.Collections.Generic;

namespace BitWaves.JudgeBoard.Utils
{
    /// <summary>
    /// Provide an implementation of double-ended queue, with optimizations for storing raw bytes as elements.
    /// </summary>
    public sealed partial class BufferDeque : IBufferDeque
    {
        private readonly LinkedList<Chunk> _chunks;
        private readonly int _chunkSize;

        /// <summary>
        /// Initialize a new <see cref="BufferDeque"/> instance.
        /// </summary>
        /// <param name="chunkSize">Size of each chunk in the internal representation.</param>
        public BufferDeque(int chunkSize)
        {
            Contract.Positive(chunkSize, nameof(chunkSize));

            _chunks = new LinkedList<Chunk>();
            _chunkSize = chunkSize;
        }

        /// <summary>
        /// Initialize a new <see cref="BufferDeque"/> instance, with a default chunk size.
        /// </summary>
        public BufferDeque()
            : this(Chunk.DefaultChunkSize)
        {
        }

        /// <summary>
        /// Add an empty chunk to the tail of the chunk list.
        /// </summary>
        private void AddChunk()
        {
            _chunks.AddLast(new Chunk(_chunkSize));
        }

        /// <summary>
        /// Pops a chunk from the head of the chunk list.
        /// </summary>
        private void PopChunk()
        {
            _chunks.RemoveFirst();
        }

        /// <inheritdoc />
        public long Size
        {
            get
            {
                if (_chunks.Count == 0)
                {
                    return 0;
                }

                if (_chunks.Count == 1)
                {
                    return _chunks.First.Value.Size;
                }

                var firstSize = _chunks.First.Value.Size;
                var lastSize = _chunks.Last.Value.Size;
                var middleSize = (long)_chunkSize * (_chunks.Count - 2);

                return firstSize + lastSize + middleSize;
            }
        }

        /// <inheritdoc />
        public void Push(ReadOnlySpan<byte> buffer)
        {
            if (_chunks.Count == 0)
            {
                AddChunk();
            }

            while (!buffer.IsEmpty)
            {
                var copied = _chunks.Last.Value.Push(buffer);
                if (copied == 0)
                {
                    AddChunk();
                }
                else
                {
                    buffer = buffer.Slice(copied);
                }
            }
        }

        /// <inheritdoc />
        public int Pop(Span<byte> buffer)
        {
            var copied = 0;
            while (!buffer.IsEmpty && _chunks.Count > 0)
            {
                var chunkCopied = _chunks.First.Value.Pop(buffer);
                if (_chunks.First.Value.IsDrained)
                {
                    PopChunk();
                }

                copied += chunkCopied;
                buffer = buffer.Slice(chunkCopied);
            }

            return copied;
        }
    }
}
