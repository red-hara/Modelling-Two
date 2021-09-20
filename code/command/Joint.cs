using Godot;

public class Joint : Command
{
    private Vector3? target;
    private (float A, float B, float C)? targetJoint;
    private (float A, float B, float C) start;
    private float velocity;
    private Controllable controllable;
    private float progress = 0;

    public Joint(Vector3 target, float velocity)
    {
        this.target = target;
        this.velocity = velocity;
    }

    public Joint((float axisA, float axisB, float axisC) targetJoint, float velocity)
    {
        this.targetJoint = targetJoint;
        this.velocity = velocity;
    }

    public void Init(Controllable controllable)
    {
        this.controllable = controllable;
    }

    public State Process(float delta)
    {
        return State.Error;
    }
}