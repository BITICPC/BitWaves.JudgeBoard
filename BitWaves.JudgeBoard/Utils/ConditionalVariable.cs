using System.Threading;

namespace BitWaves.JudgeBoard.Utils
{
    /// <summary>
    /// Implement conditional variables.
    /// </summary>
    public sealed class ConditionalVariable
    {
        private bool _state;
        private readonly object _lock;

        /// <summary>
        /// Initialize a new <see cref="ConditionalVariable"/>, with the state been initialized to <code>false</code>.
        /// </summary>
        public ConditionalVariable()
            : this(false)
        {
        }

        /// <summary>
        /// Initialize a new <see cref="ConditionalVariable"/> with the given initial state.
        /// </summary>
        /// <remarks>
        /// If the state of the conditional variable is <code>true</code>, then all threads that wait on the CV do not
        /// block; otherwise all threads that wait on the CV blocks until one or more threads call
        /// <see cref="ConditionalVariable.Notify()"/>.
        /// </remarks>
        /// <param name="state">
        /// The initial state of the conditional variable.
        /// </param>
        public ConditionalVariable(bool state)
        {
            _state = state;
            _lock = new object();
        }

        /// <summary>
        /// Set the state of the conditional variable to <code>true</code>. This results in all the waiting threads on
        /// this conditional variable been released.
        /// </summary>
        public void Notify()
        {
            lock (_lock)
            {
                _state = true;
                Monitor.PulseAll(_lock);
            }
        }

        /// <summary>
        /// Reset the state of the conditional variable to <code>false</code>. This results in all following wait
        /// operations on the conditional variable blocks.
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                if (!_state)
                {
                    return;
                }

                _state = false;
            }
        }

        /// <summary>
        /// Wait on the conditional variable until the state of the conditional variable becomes <code>true</code>.
        /// </summary>s
        public void Wait()
        {
            lock (_lock)
            {
                if (_state)
                {
                    return;
                }

                Monitor.Wait(_lock);
            }
        }
    }
}
