using System;
using BitWaves.JudgeBoard.Utils;
using NUnit.Framework;

namespace BitWaves.JudgeBoard.UnitTest
{
    /// <summary>
    /// Unit tests of <see cref="BufferDeque"/> class.
    /// </summary>
    public sealed class BufferDequeTests
    {
        [Test]
        public void SizeOnEmpty()
        {
            var deque = new BufferDeque();
            Assert.AreEqual(0, deque.Size);
        }

        [Test]
        public void SizeOnSingleChunk()
        {
            var deque = new BufferDeque(32);
            deque.Push(new Span<byte>(new byte[16]));

            Assert.AreEqual(16, deque.Size);
        }

        [Test]
        public void SizeOnTwoChunks()
        {
            var deque = new BufferDeque(16);
            deque.Push(new Span<byte>(new byte[20]));

            Assert.AreEqual(20, deque.Size);
        }

        [Test]
        public void SizeOnMoreChunks()
        {
            var deque = new BufferDeque(32);
            deque.Push(new Span<byte>(new byte[1000]));

            Assert.AreEqual(1000, deque.Size);
        }

        [Test]
        public void PushEmptySpan()
        {
            var deque = new BufferDeque();
            deque.Push(new Span<byte>());

            Assert.AreEqual(0, deque.Size);
        }

        [Test]
        public void PushToOneChunk()
        {
            var r = new Random();
            var deque = new BufferDeque(32);

            var input = new Span<byte>(new byte[32]);
            r.NextBytes(input);
            deque.Push(input);

            Assert.AreEqual(32, deque.Size);
        }

        [Test]
        public void PushToMoreChunks()
        {
            var r = new Random();
            var deque = new BufferDeque(32);

            var input = new Span<byte>(new byte[1000]);
            r.NextBytes(input);
            deque.Push(input);

            Assert.AreEqual(1000, deque.Size);
        }

        [Test]
        public void PopToEmptySpan()
        {
            var deque = new BufferDeque();
            var input = new Span<byte>(new byte[32]);
            deque.Push(input);

            Assert.AreEqual(0, deque.Pop(new Span<byte>()));
            Assert.AreEqual(32, deque.Size);
        }

        [Test]
        public void PopToSmallSpan()
        {
            var r = new Random();
            var deque = new BufferDeque();

            var input = new Span<byte>(new byte[32]);
            r.NextBytes(input);
            deque.Push(input);

            var output = new Span<byte>(new byte[16]);
            Assert.AreEqual(16, deque.Pop(output));
            Assert.AreEqual(16, deque.Size);

            for (var i = 0; i < output.Length; ++i)
            {
                Assert.AreEqual(input[i], output[i]);
            }
        }

        [Test]
        public void EmptyBufferPopToSpan()
        {
            var deque = new BufferDeque();
            Assert.AreEqual(0, deque.Pop(new Span<byte>(new byte[16])));
        }

        [Test]
        public void PopToLargeSpan()
        {
            var r = new Random();
            var deque = new BufferDeque();

            var input = new Span<byte>(new byte[32]);
            r.NextBytes(input);
            deque.Push(input);

            var output = new Span<byte>(new byte[64]);
            Assert.AreEqual(32, deque.Pop(output));
            Assert.AreEqual(0, deque.Size);

            for (var i = 0; i < input.Length; ++i)
            {
                Assert.AreEqual(input[i], output[i]);
            }
        }
    }
}
