namespace FSMSharp
{
  public readonly struct FSM_Snapshot<T>
  {
    internal T CurrentState { get; }
    internal float StateAge { get; }

    internal FSM_Snapshot(float age, T currentState)
    {
      CurrentState = currentState;
      StateAge = age;
    }
  }
}
