using JetBrains.Annotations;

namespace FSMSharp
{
  /// <summary>
  /// Extension methods to easily use the FSM as a queueing of consecutive states.
  /// </summary>
  [UsedImplicitly]
  // ReSharper disable once InconsistentNaming
  public static class FSM_QueueExtensions
  {
    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<int> Queue(this FSM<int> fsm)
    {
      return fsm.Add(fsm.Count)
        .GoesTo(fsm.Count);
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<long> Queue(this FSM<long> fsm)
    {
      return fsm.Add(fsm.Count)
        .GoesTo(fsm.Count);
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<uint> Queue(this FSM<uint> fsm)
    {
      return fsm.Add((uint)fsm.Count)
        .GoesTo((uint)(fsm.Count));
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<ulong> Queue(this FSM<ulong> fsm)
    {
      return fsm.Add((ulong)fsm.Count)
        .GoesTo((ulong)(fsm.Count));
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<short> Queue(this FSM<short> fsm)
    {
      return fsm.Add((short)fsm.Count)
        .GoesTo((short)(fsm.Count));
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<ushort> Queue(this FSM<ushort> fsm)
    {
      return fsm.Add((ushort)fsm.Count)
        .GoesTo((ushort)(fsm.Count));
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<byte> Queue(this FSM<byte> fsm)
    {
      return fsm.Add((byte)fsm.Count)
        .GoesTo((byte)(fsm.Count));
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<sbyte> Queue(this FSM<sbyte> fsm)
    {
      return fsm.Add((sbyte)fsm.Count)
        .GoesTo((sbyte)(fsm.Count + 1));
    }

    /// <summary>
    /// Queues a new state to specified FSM.
    /// </summary>
    /// <param name="fsm">The FSM.</param>
    /// <returns>The newly created state.</returns>
    [UsedImplicitly]
    public static FSM_StateBehaviour<char> Queue(this FSM<char> fsm)
    {
      return fsm.Add((char)fsm.Count)
        .GoesTo((char)(fsm.Count + 1));
    }
  }
}
