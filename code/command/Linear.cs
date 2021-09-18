using Godot;

public class Linear : Command
{
    private Vector3 start;
    private Vector3 target;
    private float velocity;
    private float progress = 0;
    private Controllable controllable;

    public Linear(Vector3 target, float velocity)
    {
        this.target = target;
        this.velocity = velocity;
    }

    public void Init(Controllable controllable)
    {
        start = controllable.GetCurrentPosition();
        this.controllable = controllable;
    }
    public State Process(float delta)
    {
        Vector3 deltaVector = target - start;
        float length = deltaVector.Length();
        if (length == 0f)
        {
            return State.Done;
        }

        progress += velocity / length * delta;
        if (progress >= 1)
        {
            if (controllable.SetPosition(target) is null)
            {
                return State.Error;
            }
            else
            {
                return State.Done;
            }
        }
        Vector3 current = start.LinearInterpolate(target, progress);
        if (controllable.SetPosition(current) is null) {
            return State.Error;
        }
        return State.Going;
    }
}