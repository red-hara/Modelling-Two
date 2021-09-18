public class Wait : Command
{
    private float counter;
    private float total;

    public Wait(float total)
    {
        this.total = total;
        counter = 0;
    }
    public void Init(Controllable controllable) { }
    public State Process(float delta)
    {
        counter += delta;
        if (counter > total)
        {
            return State.Done;
        }
        return State.Going;
    }
}