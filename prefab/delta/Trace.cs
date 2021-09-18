using Godot;
using System.Collections.Generic;

[Tool]
public class Trace : ImmediateGeometry
{
    [Export]
    public NodePath tracePath;

    private int pointCount = 1_000;

    private LinkedList<Vector3> points = new LinkedList<Vector3>();

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        Spatial traceObject = GetNode<Spatial>(tracePath);
        if (!(traceObject is null))
        {
            points.AddLast(traceObject.Translation);
        }
        while (points.Count > pointCount) {
            points.RemoveFirst();
        }
        Clear();
        Begin(Mesh.PrimitiveType.LineStrip);
        foreach (Vector3 point in points) {
            AddVertex(point);
        }
        End();
    }
}
