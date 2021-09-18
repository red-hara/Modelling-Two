using Godot;
using System.Collections.Generic;

public class Controller : Node
{
    [Export]
    public NodePath controllablePath;

    private LinkedList<Command> commands = new LinkedList<Command>();
    private Command currentCommand;

    public override void _Ready()
    {
        float velocity = 250;
        AddCommand(new Linear(new Vector3(0, -400, -600), velocity));
        AddCommand(new Linear(new Vector3(-400, -400, -600), velocity));
        AddCommand(new Wait(0.5f));
        AddCommand(new Linear(new Vector3(-400, 400, -600), velocity));
        AddCommand(new Wait(0.5f));
        AddCommand(new Linear(new Vector3(400, 400, -600), velocity));
        AddCommand(new Wait(0.5f));
        AddCommand(new Linear(new Vector3(400, -400, -600), velocity));
        AddCommand(new Wait(1));
        AddCommand(new Linear(new Vector3(0, 0, -1000), velocity * 4));
    }

    public override void _Process(float delta)
    {
        Controllable controllable = GetNode<Controllable>(controllablePath);
        if (!(currentCommand is null))
        {
            State state = currentCommand.Process(delta);
            switch (state)
            {
                case State.Going:
                    break;
                case State.Done:
                    currentCommand = null;
                    break;
                case State.Error:
                    currentCommand = null;
                    commands.Clear();
                    GD.Print("Error!");
                    break;
            }
        }
        else if (commands.Count > 0)
        {
            currentCommand = commands.First.Value;
            commands.RemoveFirst();
            currentCommand.Init(controllable);
        }
    }

    public void AddCommand(Command command)
    {
        commands.AddLast(command);
    }
}
