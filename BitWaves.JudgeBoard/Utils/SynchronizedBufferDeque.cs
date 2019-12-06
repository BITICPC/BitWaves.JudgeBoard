using System;

namespace BitWaves.JudgeBoard.Utils
{
    /// <summary>
    /// Implements a blocking version of <see cref="IBufferDeque"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     The pop operation blocks if no data is available at current time.
    ///     </para>
    ///
    ///     <para>
    ///     This class is not thread safe by means that data races might occur if multiple thread tries to pop data from
    ///     this buffer deque simultaneously.
    ///     </para>
    /// </remarks>
    public sealed class SynchronizedBufferDeque : IBufferDeque, IDisposable
    {
        private readonly IBufferDeque _inner;
        private readonly object _accessLock;
        private readonly ConditionalVariable _cv;
        private bool _disposed;

        /// <summary>
        /// Initialize a new <see cref="SynchronizedBufferDeque"/> instance.
        /// </summary>
        /// <param name="inner">The inner buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="inner"/> is null.</exception>
        public SynchronizedBufferDeque(IBufferDeque inner)
        {
            Contract.NotNull(inner, nameof(inner));

            _inner = inner;
            _accessLock = new object();
            _cv = new ConditionalVariable(false);
            _disposed = false;
        }

        /// <inheritdoc />
        public long Size
        {
            get
            {
                lock (_accessLock)
                {
                    return _inner.Size;
                }
            }
        }

        /// <summary>
        /// Ensures that current object has not disposed yet.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If the current object has been disposed.</exception>
        private void EnsureNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <inheritdoc />
        public void Push(ReadOnlySpan<byte> buffer)
        {
            lock (_accessLock)
            {
                EnsureNotDisposed();

                _inner.Push(buffer);

                if (_inner.Size > 0)
                {
                    _cv.Notify();
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method blocks if no more data is available.
        ///
        /// This method is not thread safe. The caller should guarantee that only one thread is calling this method
        /// at any time; otherwise the behavior might be unexpected.
        /// </remarks>
        public int Pop(Span<byte> buffer)
        {
            _cv.Wait();

            lock (_accessLock)
            {
                EnsureNotDisposed();

                var returnValue = _inner.Pop(buffer);
                if (_inner.Size == 0)
                {
                    // No more data available. Reset the conditional variable.
                    _cv.Reset();
                }

                return returnValue;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method is used to free any blocking thread in the <see cref="Pop"/> method.
        /// </remarks>
        public void Dispose()
        {
            lock (_accessLock)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _cv.Notify();
                }
            }
        }
    }
}
