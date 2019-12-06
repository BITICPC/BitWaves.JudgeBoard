using System;
using System.IO;

namespace BitWaves.JudgeBoard.Utils
{
    /// <summary>
    /// Provide a implementation of <see cref="Stream"/> that provides stream-based, byte-oriented producer consumer
    /// pattern.
    /// </summary>
    public sealed class ProducerConsumerStream : Stream
    {
        private readonly SynchronizedBufferDeque _buffer;

        /// <summary>
        /// Initialize a new <see cref="ProducerConsumerStream"/> instance.
        /// </summary>
        /// <param name="buffer">The internal buffer.</param>
        /// <param name="canWrite">
        /// A value indicating whether the initialized <see cref="ProducerConsumerStream"/> instance should be used as
        /// the write end.
        /// </param>
        private ProducerConsumerStream(SynchronizedBufferDeque buffer, bool canWrite)
        {
            Contract.NotNull(buffer, nameof(buffer));

            _buffer = buffer;
            CanWrite = canWrite;
        }

        /// <inheritdoc />
        public override bool CanRead => !CanWrite;

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanWrite { get; }

        /// <inheritdoc />
        public override long Length =>
            throw new InvalidOperationException($"Cannot get length of {nameof(ProducerConsumerStream)}.");

        /// <inheritdoc />
        public override long Position
        {
            get => throw new InvalidOperationException($"Cannot get length of {nameof(ProducerConsumerStream)}.");
            set => throw new InvalidOperationException($"Cannot set length of {nameof(ProducerConsumerStream)}.");
        }

        /// <summary>
        /// Ensures that the current <see cref="ProducerConsumerStream"/> is the read end of a pair of
        /// <see cref="ProducerConsumerStream"/> objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the current object is not the read end.</exception>
        private void EnsureImReader()
        {
            if (!CanRead)
                throw new InvalidOperationException("I'm not a reader.");
        }

        /// <summary>
        /// Ensures that the current <see cref="ProducerConsumerStream"/> is the write end of a pair of
        /// <see cref="ProducerConsumerStream"/> objects.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the current object is not write read end.</exception>
        private void EnsureImWriter()
        {
            if (!CanWrite)
                throw new InvalidOperationException("I'm not a writer.");
        }

        /// <inheritdoc />
        public override void Flush()
        {
            EnsureImWriter();
        }

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count)
        {
            EnsureImReader();

            return _buffer.Pop(new Span<byte>(buffer, offset, count));
        }

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException($"Cannot seek on {nameof(ProducerConsumerStream)}.");
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            throw new InvalidOperationException($"Cannot set length of {nameof(ProducerConsumerStream)}.");
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            EnsureImWriter();

            _buffer.Push(new ReadOnlySpan<byte>(buffer, offset, count));
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (CanWrite)
            {
                // The internal buffer should be disposed by the writer.
                _buffer.Dispose();
            }
        }

        /// <summary>
        /// Create a new pair of <see cref="ProducerConsumerStream"/>.
        /// </summary>
        /// <returns>
        /// A tuple containing two <see cref="ProducerConsumerStream"/> instance. The first field of the tuple is the
        /// write end (the producer) and the second field is the read end (the consumer).
        /// </returns>
        public static (ProducerConsumerStream Producer, ProducerConsumerStream Consumer) Create()
        {
            var buffer = new SynchronizedBufferDeque(new BufferDeque());
            var producer = new ProducerConsumerStream(buffer, false);
            var consumer = new ProducerConsumerStream(buffer, true);

            return (producer, consumer);
        }
    }
}
