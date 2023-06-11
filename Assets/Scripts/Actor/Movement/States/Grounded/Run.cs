using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class Run : Walk
    {
        public Run(StateMachine stateMachine) : base(stateMachine)
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

            if (!_tiesToRun && _targetMoveInput.magnitude != 0)
            {
                _stateMachine.SwitchState(_stateMachine.WalkState);
                return;
            }

            if (_currentMoveInput.magnitude == 0 && _targetMoveInput.magnitude == 0)
            {
                _stateMachine.SwitchState(_stateMachine.IdleState);
                return;
            }
        }

        protected override void UpdateMove()
        {
            UpdateTargetMoveInputVector(_stateMachine.Data.Run.Speed);
            UpdateCurrentMoveInputVector(_stateMachine.Data.Run.Acceleration, _stateMachine.Data.Run.Deceleration);
            var moveVector = GetMoveVectorFromInput(_currentMoveInput);

            if (ShouldCorrectToSlope()) moveVector = AdjustMoveToGroundAngle(moveVector);

            moveVector.y -= _stateMachine.Data.Walk.DownForce;
            Move(moveVector);
        }
    }
}