using Godot;
using System;

public interface Jointable
{
    Vector3? SetJoints((float A, float B, float C) joints);
    (float A, float B, float C) GetCurrentJoints();
}
