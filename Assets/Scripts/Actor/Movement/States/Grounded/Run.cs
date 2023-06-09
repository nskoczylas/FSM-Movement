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
            CheckIsFalling(0.1f);
            CheckIsSlidingDownSlope();

            if (!_tiesToRun && _moveInputVector.magnitude != 0)
            {
                _stateMachine.SwitchState(_stateMachine.WalkState);
                return;
            }
            
            if (_currentMoveInputVector.magnitude == 0 && _moveInputVector.magnitude == 0) _stateMachine.SwitchState(_stateMachine.IdleState);
        }

        protected override void UpdateMove()
        {
            _moveInputVector = GetMoveInputVector(_stateMachine.MovementSettings.Run);
            _currentMoveInputVector = GetCurrentMoveInputVector(_stateMachine.MovementSettings.Run);
            var moveVector = GetMoveVectorFromInput(_currentMoveInputVector);

            if (ShouldCorrectToSlope()) moveVector = AdjustMoveToGroundAngle(moveVector);

            moveVector.y -= _stateMachine.MovementSettings.Walk.DownForce;
            Move(moveVector);
        }
    }
}