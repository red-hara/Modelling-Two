using Godot;
using System;

[Tool]
public class ControlledDelta : Spatial, Controllable
{
    [Export]
    public NodePath deltaPath;
    private Delta robot;

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
    public float axisLimitPositive = 10;
    [Export]
    public float axisLimitNegative = -120;

    public override void _Ready()
    {
        robot = GetNode<Delta>(deltaPath);
    }
    public override void _Process(float delta)
    {
        if (attachedPath != null)
        {
            Spatial attach = GetNode<Spatial>(attachedPath);
            attach.Translation = robot.Position;
        }
    }

    public (float A, float B, float C)? Inverse(Vector3 target)
    {
        target += new Vector3(0, 0, 25);
        float? a = InverseAxis(target, 0);
        float? b = InverseAxis(target, 1);
        float? c = InverseAxis(target, 2);
        if (a is null || b is null || c is null)
        {
            return null;
        }
        float axisA = Mathf.Rad2Deg(a ?? default);
        float axisB = Mathf.Rad2Deg(b ?? default);
        float axisC = Mathf.Rad2Deg(c ?? default);
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

    private static float? InverseAxis(Vector3 position, int index)
    {
        Quat rotation = new Quat(Vector3.Back, Mathf.Pi * 2 / 3 * index);
        Vector3 axisU = Vector3.Forward;
        Vector3 axisV = rotation.Xform(Vector3.Left);
        Vector3 circleCenter = rotation.Xform(new Vector3(Delta.baseRadius - Delta.platformRadius, 0, Delta.baseLift));
        float circleRadius = Delta.armLength;
        var maybeSolution = Geometry.SphereCircleIntersectionAngles(Delta.connectorRadius, position, circleCenter, axisU, axisV, circleRadius);
        if (maybeSolution is null)
        {
            return null;
        }
        var solution = maybeSolution ?? default;
        if (Mathf.Wrap(solution.SolutionA, -Mathf.Pi, Mathf.Pi) < Mathf.Wrap(solution.SolutionB, -Mathf.Pi, Mathf.Pi))
        {
            return solution.SolutionA;
        }
        return solution.SolutionB;
    }

    public (float A, float B, float C)? SetPosition(Vector3 position)
    {
        var maybeSolution = Inverse(position);
        if (!(maybeSolution is null) && !(robot is null))
        {
            var solution = maybeSolution ?? (0, 0, 0);
            robot.axisA = solution.A;
            robot.axisB = solution.B;
            robot.axisC = solution.C;
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
