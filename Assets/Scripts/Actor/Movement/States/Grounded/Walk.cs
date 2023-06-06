using FSM;

namespace Actor.Movement.States.Grounded
{
    public class Walk : Idle
    {
        public Walk(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void CheckSwitchState()
        {
            CheckIsFalling(1.5f);
            if (_stateMachine.MovementInput.Move.magnitude == 0) _stateMachine.SwitchState(_stateMachine.IdleState);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}