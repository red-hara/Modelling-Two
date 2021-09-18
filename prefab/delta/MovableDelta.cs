using Godot;
using System;

[Tool]
public class MovableDelta : Spatial
{
    [Export]
    public NodePath robotPath;

    private float counter;

    public override void _Ready()
    {

    }


    public override void _Process(float delta)
    {
        counter += delta;

        Controllable robot = GetNode<Controllable>(robotPath);
        if (robot != null)
        {
            robot.SetJoints((
                -60 + 60 * Mathf.Sin(counter),
                -60 + 60 * Mathf.Sin(counter + Mathf.Pi * 2 / 3),
                -60 + 60 * Mathf.Sin(counter + Mathf.Pi * 4 / 3)
            ));
        }
    }
}
