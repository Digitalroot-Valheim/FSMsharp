using System;
using System.Collections.Generic;

namespace FSMSharp
{
  /// <summary>
  /// A Finite State Machine. 
  /// T is a type which will be used as descriptors of the state. Usually this is an enum, string or an integral type,
  /// but any type can be used.
  /// </summary>
  /// <typeparam name="T">A type which will be used as descriptors of the state. Usually this is an enum, string or an integral type,
  /// but any type can be used.</typeparam>
  public class FSM<T>
  {
    private readonly Dictionary<T, FSM_StateBehaviour<T>> _stateBehaviours = new();
    private T _currentState;
    private FSM_StateBehaviour<T> _currentStateBehaviour;
    private float _stateAge = -1f;
    private readonly string _fsmName;
    private float _timeBaseForIncremental;

    /// <summary>
    /// Initializes a new instance of the <see cref="FSM{T}"/> class.
    /// </summary>
    /// <param name="name">The name of the FSM, used in throw exception and for debug purposes.</param>
    public FSM(string name)
    {
      _fsmName = name;
    }

    /// <summary>
    /// Gets or sets a callback which will be called when the FSM logs state transitions. Used to track state transition for debug purposes.
    /// </summary>
    /// <value>
    /// The debug log handler.
    /// </value>
    public Action<string> DebugLogHandler { get; set; }

    /// <summary>
    /// Adds the specified state.
    /// </summary>
    /// <param name="state">The state.</param>
    /// <returns>The newly created behaviour, so that it could be configured with a fluent-like syntax.</returns>
    public FSM_StateBehaviour<T> Add(T state)
    {
      var behaviour = new FSM_StateBehaviour<T>(state);
      _stateBehaviours.Add(state, behaviour);
      return behaviour;
    }

    /// <summary>
    /// Gets the number of states currently in the FSM.
    /// </summary>
    /// <value>
    /// The number of states currently in the FSM.
    /// </value>
    public int Count => _stateBehaviours.Count;

    /// <summary>
    /// Processes the logic for the FSM. 
    /// </summary>
    /// <param name="time">The time, expressed in seconds.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void ProcessIncremental(float dtime)
    {
      _timeBaseForIncremental += dtime;
      Process(_timeBaseForIncremental);
    }

    /// <summary>
    /// Processes the logic for the FSM. 
    /// </summary>
    /// <param name="time">The time, expressed in seconds.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(float time)
    {
      if (_stateAge < 0f) _stateAge = time;

      var totalTime = time;
      var stateTime = (totalTime - _stateAge);
      var stateProgress = 0f;

      if (_currentStateBehaviour == null)
      {
        throw new InvalidOperationException($"[FSM {_fsmName}] : Can't call 'Process' before setting the starting state.");
      }

      if (_currentStateBehaviour.Duration.HasValue)
      {
        stateProgress = Math.Max(0f, Math.Min(1f, stateTime / _currentStateBehaviour.Duration.Value));
      }

      var data = new FSM_StateData<T>
      {
        Machine = this, Behaviour = _currentStateBehaviour, State = _currentState, StateTime = stateTime, AbsoluteTime = totalTime, StateProgress = stateProgress
      };

      _currentStateBehaviour.Trigger(data);

      if (stateProgress >= 1f && _currentStateBehaviour.NextStateSelector != null)
      {
        CurrentState = _currentStateBehaviour.NextStateSelector();
        _stateAge = time;
      }
    }

    /// <summary>
    /// Gets or sets the current state of the FSM.
    /// </summary>
    public T CurrentState
    {
      get => _currentState;
      set => InternalSetCurrentState(value, true);
    }

    private void InternalSetCurrentState(T value, bool executeSideEffects)
    {
      DebugLogHandler?.Invoke($"[FSM {_fsmName}] : Changing state from {_currentState} to {value}");

      if (_currentStateBehaviour != null && executeSideEffects)
      {
        _currentStateBehaviour.TriggerLeave();
      }

      _stateAge = -1f;

      _currentStateBehaviour = _stateBehaviours[value];
      _currentState = value;

      if (_currentStateBehaviour != null && executeSideEffects)
      {
        _currentStateBehaviour.TriggerEnter();
      }
    }

    /// <summary>
    /// Moves the FSM to the next state as configured using FSM_StateBehaviour.GoesTo(...).
    /// Note: to change the state freely, use the CurrentState property.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the behaviour has not a next state / state selector configured.</exception>
    public void Next()
    {
      if (_currentStateBehaviour.NextStateSelector != null)
      {
        CurrentState = _currentStateBehaviour.NextStateSelector();
      }
      else
      {
        throw new InvalidOperationException($"[FSM {_fsmName}] : Can't call 'Next' on current behaviour.");
      }
    }

    /// <summary>
    /// Saves a snapshot of the FSM
    /// </summary>
    /// <returns>The snapshot.</returns>
    public FSM_Snapshot<T> SaveSnapshot()
    {
      return new FSM_Snapshot<T>(_stateAge, _currentState);
    }

    /// <summary>
    /// Restores a snapshot of the FSM taken with SaveSnapshot
    /// </summary>
    /// <param name="snap">The snapshot.</param>
    /// <param name="executeSideEffects"></param>
    public void RestoreSnapshot(FSM_Snapshot<T> snap, bool executeSideEffects)
    {
      InternalSetCurrentState(snap.CurrentState, executeSideEffects);
      _stateAge = snap.StateAge;
    }

    public FSM_StateBehaviour<T> GetBehaviour(T state)
    {
      return _stateBehaviours[state];
    }
  }
}
