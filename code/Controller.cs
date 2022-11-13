using Godot;
using System.Collections.Generic;

/// <summary>The <c>Controller</c> node controls the <c>Controllable</c>
/// node.</summary>
public class Controller : Node
{
    /// <summary>The <Controllable> to be controlled</summary>
    [Export]
    public NodePath controllablePath;

    private LinkedList<Command> commands = new LinkedList<Command>();
    private Command currentCommand;

    public override void _Ready()
    {
        float velocity = 250;
        AddCommand(new Linear(new Vector3(0, -200, -250), velocity));
        AddCommand(new Linear(new Vector3(-200, -200, -250), 2 * velocity));
        AddCommand(new Wait(0.5f));
        AddCommand(new Linear(new Vector3(-200, 200, -250), velocity));
        AddCommand(new Wait(0.5f));
        AddCommand(new Linear(new Vector3(200, 200, -250), 2 * velocity));
        AddCommand(new Wait(0.5f));
        AddCommand(new Linear(new Vector3(200, -200, -250), velocity));
        AddCommand(new Wait(1));
        AddCommand(new Linear(new Vector3(0, 0, -500), 4 * velocity));
    }

    public override void _Process(float delta)
    {
        // Get controllable.
        Controllable controllable = GetNode<Controllable>(controllablePath);
        if (!(currentCommand is null))
        {
            // Process current command if it is present.
            State state = currentCommand.Process(delta);
            // Handle command state.
            switch (state)
            {
                case State.Going:
                    break;
                case State.Done:
                    currentCommand = null;
                    break;
                case State.Error:
                    // In case of an error just ditch all following commands.
                    currentCommand = null;
                    commands.Clear();
                    GD.Print("Error!");
                    break;
            }
        }
        // If we have no current command just take one from the queue.
        else if (commands.Count > 0)
        {
            currentCommand = commands.First.Value;
            commands.RemoveFirst();
            // And init it.
            currentCommand.Init(controllable);
        }
    }

    /// <summary>Add command to command queue.</summary>
    /// <param name="command">A command instance to be added.</param>
    public void AddCommand(Command command)
    {
        commands.AddLast(command);
    }
}
