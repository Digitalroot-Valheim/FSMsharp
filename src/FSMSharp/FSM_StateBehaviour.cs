using System;
using System.Collections.Generic;

namespace FSMSharp
{
    /// <summary>
    /// Defines the behaviour of a state of a finite state machine
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FSM_StateBehaviour<T>
    {
        private readonly List<Action<FSM_StateData<T>>> _processCallbacks = new();
        private readonly List<Action> _enterCallbacks = new();
        private readonly List<Action> _leaveCallbacks = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FSM_StateBehaviour{T}"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        internal FSM_StateBehaviour(T state)
        {
            State = state;
        }

        /// <summary>
        /// Gets the state associated with this behaviour
        /// </summary>
        public T State { get; }

        /// <summary>
        /// Gets the time duration of the state (if any)
        /// </summary>
        public float? Duration { get; private set; }

        /// <summary>
        /// Gets the function which will be used to select the next state when this expires or Next() gets called.
        /// </summary>
        public Func<T> NextStateSelector { get; private set; }

        /// <summary>
        /// Sets a callback which will be called when the FSM enters in this state
        /// </summary>
        public FSM_StateBehaviour<T> OnEnter(Action callback)
        {
            _enterCallbacks.Add(callback);
            return this;
        }

        /// <summary>
        /// Sets a callback which will be called when the FSM leaves this state
        /// </summary>
        public FSM_StateBehaviour<T> OnLeave(Action callback)
        {
            _leaveCallbacks.Add(callback);
            return this;
        }

        /// <summary>
        /// Sets a callback which will be called every time Process is called on the FSM, when this state is active
        /// </summary>
        public FSM_StateBehaviour<T> Calls(Action<FSM_StateData<T>> callback)
        {
            _processCallbacks.Add(callback);
            return this;
        }

        /// <summary>
        /// Sets the state to automatically expire after the given time (in seconds)
        /// </summary>
        public FSM_StateBehaviour<T> Expires(float duration)
        {
            Duration = duration;
            return this;
        }

        /// <summary>
        /// Sets the state to which the FSM goes when the duration of this expires, or when Next() gets called on the FSM
        /// </summary>
        /// <param name="state">The state.</param>
        public FSM_StateBehaviour<T> GoesTo(T state)
        {
            NextStateSelector = () => state;
            return this;
        }

        /// <summary>
        /// Sets a function which selects the state to which the FSM goes when the duration of this expires, or when Next() gets called on the FSM
        /// </summary>
        /// <param name="stateSelector">The state selector function.</param>
        public FSM_StateBehaviour<T> GoesTo(Func<T> stateSelector)
        {
            NextStateSelector = stateSelector;
            return this;
        }

        /// <summary>
        /// Calls the process callback
        /// </summary>
        internal void Trigger(FSM_StateData<T> data)
        {
            for (int i = 0, len = _processCallbacks.Count; i < len; i++)
                _processCallbacks[i](data);
        }

        /// <summary>
        /// Calls the onenter callback
        /// </summary>
        internal void TriggerEnter()
        {
            for (int i = 0, len = _enterCallbacks.Count; i < len; i++)
                _enterCallbacks[i]();
        }

        /// <summary>
        /// Calls the onleave callback
        /// </summary>
        internal void TriggerLeave()
        {
            for (int i = 0, len = _leaveCallbacks.Count; i < len; i++)
                _leaveCallbacks[i]();
        }
    }
}
