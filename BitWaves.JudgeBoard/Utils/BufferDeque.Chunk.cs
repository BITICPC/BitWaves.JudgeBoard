using System;

namespace BitWaves.JudgeBoard.Utils
{
    public sealed partial class BufferDeque
    {
        /// <summary>
        /// A chunk in the internal structure of <see cref="BufferDeque"/>.
        /// </summary>
        /// <remarks>
        /// A <see cref="BufferDeque"/> instance constitutes a linked list of <see cref="Chunk"/>s, each
        /// <see cref="Chunk"/> contains some number of bytes of raw data. Each chunk can be manipulated independently
        /// by pushing raw bytes to the end of the chunk or popping raw bytes from the start of the chunk.
        /// </remarks>
        private sealed class Chunk
        {
            /// <summary>
            /// Size of each chunk.
            /// </summary>
            public const int DefaultChunkSize = 32;

            private byte[] _buffer;
            private int _reader;
            private int _writer;

            /// <summary>
            /// Initialize a new <see cref="Chunk"/> instance.
            /// </summary>
            public Chunk()
                : this(DefaultChunkSize)
            {
            }

            /// <summary>
            /// Initialize a new <see cref="Chunk"/> instance.
            /// </summary>
            /// <param name="chunkSize">The size of the chunk.</param>
            public Chunk(int chunkSize)
            {
                Contract.Positive(chunkSize, nameof(chunkSize));

                _buffer = new byte[chunkSize];
                _reader = 0;
                _writer = 0;
            }

            /// <summary>
            /// Get the size of the chunk, i.e. the available number of bytes contained in the chunk.
            /// </summary>
            /// <returns>The size of the chunk.</returns>
            public int Size => _writer - _reader;

            /// <summary>
            /// Get the free size of the chunk, i.e. the available space after <see cref="_writer"/> till end of the
            /// chunk.
            /// </summary>
            /// <returns>The free size of the chunk.</returns>
            private int FreeSize => _buffer.Length - _writer;

            /// <summary>
            /// Determine whether the chunk is empty.
            /// </summary>
            public bool IsEmpty => Size == 0;

            /// <summary>
            /// Determine whether the chunk is drained. A chunk is drained if it's empty and no free space is available
            /// after the write pointer.
            /// </summary>
            public bool IsDrained => IsEmpty && FreeSize == 0;

            /// <summary>
            /// Read raw bytes from the start of the chunk and pops them out of the chunk.
            /// </summary>
            /// <param name="buffer">The output buffer.</param>
            /// <returns>Number of bytes actually popped.</returns>
            public int Pop(Span<byte> buffer)
            {
                var copySize = Math.Min(buffer.Length, Size);
                unsafe
                {
                    fixed (byte* source = _buffer)
                    fixed (byte* dest = buffer)
                    {
                        Buffer.MemoryCopy(source, dest + _reader, copySize, copySize);
                    }
                }

                _reader += copySize;
                return copySize;
            }

            /// <summary>
            /// Copy raw data from the given input buffer to the tail of this chunk.
            /// </summary>
            /// <param name="buffer">The input buffer.</param>
            /// <returns>Number of bytes actually copied.</returns>
            public int Push(ReadOnlySpan<byte> buffer)
            {
                var copySize = Math.Min(buffer.Length, FreeSize);
                unsafe
                {
                    fixed (byte* source = buffer)
                    fixed (byte* dest = _buffer)
                    {
                        Buffer.MemoryCopy(dest + _writer, source, copySize, copySize);
                    }
                }

                _writer += copySize;
                return copySize;
            }
        }
    }
}
