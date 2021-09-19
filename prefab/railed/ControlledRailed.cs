using Godot;
using System;

[Tool]
public class ControlledRailed : Spatial, Controllable
{
    [Export]
    public NodePath railedPath;
    private Railed robot;

    [Export]
    public NodePath attachedPath;

    [Export]
    public Vector3 target
    {
        get
        {
            return robot.Position;
        }
        set
        {
            SetPosition(value);
        }
    }

    [Export]
    public float axisLimitPositive = 1000;
    [Export]
    public float axisLimitNegative = 0;
    public override void _Ready()
    {
        robot = GetNode<Railed>(railedPath);
    }

    public override void _Process(float delta)
    {
        if (attachedPath != null)
        {
            Spatial attach = GetNode<Spatial>(attachedPath);
            attach.Translation = robot.Position;
        }
    }

    private (float AxisA, float AxisB, float AxisC)? Inverse(Vector3 target)
    {
        target += new Vector3(0, 0, 25);
        float? a = InverseSlider(target, 0);
        float? b = InverseSlider(target, 1);
        float? c = InverseSlider(target, 2);
        if (a is null || b is null || c is null)
        {
            return null;
        }
        float axisA = a ?? default;
        float axisB = b ?? default;
        float axisC = c ?? default;
        if (axisA > axisLimitPositive || axisA < axisLimitNegative)
        {
            return null;
        }
        if (axisB > axisLimitPositive || axisB < axisLimitNegative)
        {
            return null;
        }
        if (axisC > axisLimitPositive || axisC < axisLimitNegative)
        {
            return null;
        }
        return (axisA, axisB, axisC);
    }

    private float? InverseSlider(Vector3 position, int index)
    {
        Vector3 p1 = Railed.SliderPosition(0, index);
        Vector3 p2 = Railed.SliderPosition(Railed.railLength, index);
        Vector3 p3 = position;
        Vector3 d21 = p2 - p1;
        Vector3 d13 = p1 - p3;
        float a = d21.LengthSquared();
        float b = 2 * d21.Dot(d13);
        float c = p3.LengthSquared() + p1.LengthSquared() - 2 * p3.Dot(p1) - Railed.connectorRadius * Railed.connectorRadius;
        float underRoot = b * b - 4 * a * c;
        if (underRoot < 0)
        {
            return null;
        }
        float t1 = (-b + Mathf.Sqrt(underRoot)) / 2 / a;
        float t2 = (-b - Mathf.Sqrt(underRoot)) / 2 / a;
        float t = (t1 > t2) ? t1 : t2;
        return t * Railed.railLength;
    }

    public (float A, float B, float C)? SetPosition(Vector3 position)
    {
        var maybeSolution = Inverse(position);
        if (!(maybeSolution is null) && !(robot is null))
        {
            var solution = maybeSolution ?? (0, 0, 0);
            robot.axisA = solution.AxisA;
            robot.axisB = solution.AxisB;
            robot.axisC = solution.AxisC;
            return (solution);
        }
        return null;
    }

    public Vector3 GetCurrentPosition()
    {
        return robot.Position;
    }

    public Vector3? SetJoints((float A, float B, float C) joints)
    {
        if (!(robot is null))
        {
            robot.axisA = joints.A;
            robot.axisB = joints.B;
            robot.axisC = joints.C;
            return robot.Forward();
        }
        return null;
    }

    public (float A, float B, float C) GetCurrentJoints()
    {
        return (robot.axisA, robot.axisB, robot.axisC);
    }
}
