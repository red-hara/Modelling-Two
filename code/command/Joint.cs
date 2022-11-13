using Godot;

/// <summary>The joint motion command.</summary>
public class Joint : Command
{
    // The target position.
    // Note that it is under the question mark.
    // It means that it may be absent.
    private Vector3? target;
    // The target joint position.
    // It is under the question marks, is should be `null` if the target joint position is unreachable.
    private (float A, float B, float C)? targetJoint;
    // The start joint position.
    // It should be initialized during `Init`.
    private (float A, float B, float C) start;
    // Motion speed in range (0, 1].
    private float speed;
    // The controllable entity.
    private Controllable controllable;
    // Internal progress counter.
    private float progress = 0;

    // This block is done properly.
    public Joint(Vector3 target, float velocity)
    {
        this.target = target;
        this.speed = velocity;
    }

    // This block is done properly.
    public Joint((float axisA, float axisB, float axisC) targetJoint, float velocity)
    {
        this.targetJoint = targetJoint;
        this.speed = velocity;
    }

    public void Init(Controllable controllable)
    {
        this.controllable = controllable;
        // Here you'll need to:
        // - Initialize `start`;
        // - Initialize `targetJoint` if the `target` is set (solve IK);
    }

    public State Process(float delta)
    {
        // Here you'll need to:
        // - Check if `targetJoint` is set and return error if it is not;
        // - Calculate maximum (absolute) distance to normalize the joint motion
        //   by;
        //     - If it equals to zero report that we are done;
        // - Calculate the progress increment;
        // - Handle the case when the progress is greater than 1;
        // - Interpolate generalized coordinates, individually, each by the
        //   progress;
        // - Set the controllable generalized coordinates to interpolated
        //   values;
        // - Indicate that we are still working;
        return State.Error;
    }
}
