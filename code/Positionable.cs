using Godot;
using System;

public interface Positionable
{
    (float A, float B, float C)? SetPosition(Vector3 position);
    (float A, float B, float C)? Inverse(Vector3 position);
    Vector3 GetCurrentPosition();
}
