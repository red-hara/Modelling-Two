using Godot;
using System;

[Tool]
public class Railed : Spatial
{
    public const float railLength = 1000;
    public const float platformRadius = 120;
    public const float railRadius = 620;
    public const float connectorRadius = 800;

    [Export]
    public float axisA;
    [Export]
    public float axisB;
    [Export]
    public float axisC;

    [Export]
    public NodePath sliderAPath;
    private Spatial sliderA;

    [Export]
    public NodePath sliderBPath;
    private Spatial sliderB;

    [Export]
    public NodePath sliderCPath;
    private Spatial sliderC;

    [Export]
    public NodePath platformPath;
    private Spatial platform;

    [Export]
    private NodePath connectorPathAA;
    [Export]
    private NodePath connectorPathAB;
    [Export]
    private NodePath connectorPathBA;
    [Export]
    private NodePath connectorPathBB;
    [Export]
    private NodePath connectorPathCA;
    [Export]
    private NodePath connectorPathCB;

    private Spatial[] connectors;

    private Vector3 position;
    public Vector3 Position
    {
        get
        {
            return position - new Vector3(0, 0, 25);
        }
    }

    public override void _Ready()
    {
        sliderA = GetNode<Spatial>(sliderAPath);
        sliderB = GetNode<Spatial>(sliderBPath);
        sliderC = GetNode<Spatial>(sliderCPath);
        platform = GetNode<Spatial>(platformPath);

        connectors = new Spatial[]{
            GetNode<Spatial>(connectorPathAA),
            GetNode<Spatial>(connectorPathAB),
            GetNode<Spatial>(connectorPathBA),
            GetNode<Spatial>(connectorPathBB),
            GetNode<Spatial>(connectorPathCA),
            GetNode<Spatial>(connectorPathCB),
        };
    }

    public override void _Process(float delta)
    {
        sliderA.Translation = new Vector3(0, 0, axisA);
        sliderB.Translation = new Vector3(0, 0, axisB);
        sliderC.Translation = new Vector3(0, 0, axisC);

        position = Forward() ?? platform.Translation;
        platform.Translation = position;

        ManageConnectors();
    }
    private void ManageConnectors()
    {
        float[] axis = new float[]{
            axisA,
            axisB,
            axisC
        };
        for (int index = 0; index < 3; index++)
        {
            Transform origin = SliderTransform(axis[index], index);
            Vector3 delta = platform.Translation - origin.origin;
            delta = origin.basis.Inverse().Xform(delta);

            float angleZ = Mathf.Atan2(delta.y, delta.x);
            float angleY = Mathf.Atan2(delta.z, Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y));
            connectors[index * 2].Transform = new Transform(
                new Quat(Vector3.Back, angleZ) * new Quat(Vector3.Up, -angleY - Mathf.Pi / 2),
                new Vector3(50, 0, 0)
            );
            connectors[index * 2 + 1].Transform = new Transform(
                new Quat(Vector3.Back, angleZ) * new Quat(Vector3.Up, -angleY - Mathf.Pi / 2),
                new Vector3(-50, 0, 0)
            );
        }
    }

    public Vector3? Forward()
    {
        Vector3 centerA = SliderPosition(axisA, 0);
        Vector3 centerB = SliderPosition(axisB, 1);
        Vector3 centerC = SliderPosition(axisC, 2);

        Vector3[] solution = Geometry.SphereIntersection3(centerA, centerB, centerC, connectorRadius);
        if (solution.Length == 0)
        {
            return null;
        }
        if (solution[0].z < solution[1].z)
        {
            return solution[0];
        }
        return solution[1];
    }

    public static Vector3 SliderPosition(float axis, int index)
    {
        Transform railOrigin = new Transform(new Quat(Vector3.Forward, Mathf.Deg2Rad(index * 120)), Vector3.Zero) *
            new Transform(new Quat(Vector3.Right, Mathf.Deg2Rad(30)), new Vector3(0, railRadius - platformRadius, 0));
        return railOrigin.Xform(new Vector3(0, 0, axis));
    }

    public static Transform SliderTransform(float axis, int index)
    {
        Transform railOrigin = new Transform(new Quat(Vector3.Forward, Mathf.Deg2Rad(index * 120)), Vector3.Zero) *
            new Transform(new Quat(Vector3.Right, Mathf.Deg2Rad(30)), new Vector3(0, railRadius - platformRadius, 0));
        return railOrigin * new Transform(Basis.Identity, new Vector3(0, 0, axis));
    }
}
