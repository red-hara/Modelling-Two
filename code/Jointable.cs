using Godot;

/// <summary>The <c>Jointable</c> instance provides several methods for working
/// with its joints.</summary>
public interface Jointable
{
    /// <summary>Try setting generalized coordinates for this
    /// <c>Jointable</c>.</summary>
    /// <param name="joints">Generalized coordinates to try to set.</param>
    /// <returns>Position of this <c>Jointable</c>  if operation
    /// successful.</returns>
    Vector3? SetJoints((float A, float B, float C) joints);
    /// <summary>Get current generalized coordinates.</summary>
    /// <returns>Current generalized coordinates.</returns>
    (float A, float B, float C) GetCurrentJoints();
    /// <summary>Get maximum joint velocity.</summary>
    /// <returns>Maximum joint velocity.</returns>
    float MaximumJointVelocity();
}
