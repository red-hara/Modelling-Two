public interface Command
{
    void Init(Controllable controllable);
    State Process(float delta);
}

public enum State
{
    Going,
    Done,
    Error,

}