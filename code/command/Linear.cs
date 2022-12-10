using Godot;

/// <summary>Perform linear motion.</summary>
public class Linear : Command
{
    /// <summary>The starting position in space.</summary>
    private Vector3 start;
    /// <summary>The target position in space.</summary>
    private Vector3 target;
    /// <summary>The motion velocity, millimeter per second.</summary>
    private float velocity;
    /// <summary>Internal progress counter, in range from 0 to 1.</summary>
    private float progress = 0;
    /// <summary>The controllable object to control.</summary>
    private Controllable controllable;

    /// <summary>Create new linear motion.</summary>
    /// <param name="target">The target position.</param>
    /// <param name="velocity">The motion velocity, millimeter per
    /// second.</param>
    public Linear(Vector3 target, float velocity)
    {
        this.target = target;
        this.velocity = velocity;
    }

    public void Init(Controllable controllable)
    {
        // Note that we get our starting position during initialization, right
        // before the motion starts.
        start = controllable.GetCurrentPosition();
        this.controllable = controllable;
    }
    public State Process(float delta)
    {
        // We calculate distance between points, and if it is zero report that
        // we are done.
        Vector3 deltaVector = target - start;
        float length = deltaVector.Length();
        if (length == 0f)
        {
            return State.Done;
        }

        // Increment progress by scaled delta value.
        progress += velocity / length * delta;
        // If the progress is more or equal to one mark the motion as finished
        // and move the controllable to the target position.
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
        // The motion is not finished.
        // Interpolate current position and move the controllable there.
        Vector3 current = start.LinearInterpolate(target, progress);
        // If the target is unreachable report an error.
        if (controllable.SetPosition(current) is null)
        {
            return State.Error;
        }
        return State.Going;
    }
}
