using FSM;

namespace Actor.Movement.States
{
    public abstract class MovementState : State
    {
        protected new MovementSM _stateMachine;
        
        public MovementState(StateMachine stateMachine) : base(stateMachine)
        {
            _stateMachine = (MovementSM)stateMachine;
        }
    }
}