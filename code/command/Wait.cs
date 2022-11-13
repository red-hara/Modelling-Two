/// <summary>Wait for specified time to pass.</summary>
public class Wait : Command
{
    private float counter;
    private float total;

    /// <summary>Create the command.</summary>
    /// <param name="total">The time to wait for, in seconds.<param>
    public Wait(float total)
    {
        this.total = total;
        counter = 0;
    }
    public void Init(Controllable controllable) { }
    public State Process(float delta)
    {
        // Increment the progress counter.
        counter += delta;
        // We are done if we waited enough.
        if (counter >= total)
        {
            return State.Done;
        }
        // Otherwise keep waiting.
        return State.Going;
    }
}
