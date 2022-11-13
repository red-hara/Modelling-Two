/// <summary>Generic command interface. Every command is initialized once and
/// then is polled each update tick using the <c>Process</c> method.</summary>
public interface Command
{
    /// <summary>Initialize this command before it is polled.</summary>
    /// <param name="controllable">The mechanism to be controlled.</param>
    void Init(Controllable controllable);
    /// <summary>Do unit of work of this <c>Command</c> once.</summary>
    /// <param name="delta">Time passed since previous update tick,
    /// seconds.</param>
    /// <returns>Current execution <c>State</c></returns>
    State Process(float delta);
}

/// <summary><c>Command</c> execution state.</summary>
public enum State
{
    /// <summary>The command requires to be executed again.</summary>
    Going,
    /// <summary>The command finished its execution successfully.</summary>
    Done,
    /// <summary>The command got an error during execution.</summary>
    Error,
}
