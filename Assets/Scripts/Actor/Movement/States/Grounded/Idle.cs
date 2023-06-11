using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class Idle : MovementState
    {
        public Idle(StateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void CheckSwitchState()
        {
            if (ShouldBeFalling(0.25f))
            {
                _stateMachine.SwitchState(_stateMachine.FallState);
                return;
            }

            if (ShouldBeSliding())
            {
                _stateMachine.SwitchState(_stateMachine.SlopeSlideState);
                return;
            }

            if (_stateMachine.MovementInput.Move.magnitude != 0)
            {
                _stateMachine.SwitchState(_stateMachine.WalkState);
                return;
            }
        }
    }
}