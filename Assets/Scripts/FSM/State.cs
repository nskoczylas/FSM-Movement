public abstract class State
{
    protected StateMachine _stateMachine;

    public State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void CheckSwitchState();
    public abstract void OnExit();
}