using Godot;

public class Joint : Command
{
    private Vector3? target;
    private (float axisA, float axisB, float axisC)? targetJoint;
    private float velocity;

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
    }

    public State Process(float delta)
    {
        return State.Error;
    }
}