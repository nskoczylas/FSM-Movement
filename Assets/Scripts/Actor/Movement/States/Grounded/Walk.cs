using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class Walk : Idle
    {
        protected Vector2 _moveInputVector;
        protected Vector2 _currentMoveInputVector;

        protected Vector2 _smoothVelocity;
        
        public Walk(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateMove();
        }

        public override void CheckSwitchState()
        {
            CheckIsFalling(0.1f);
            if (_currentMoveInputVector.magnitude == 0 && _moveInputVector.magnitude == 0) _stateMachine.SwitchState(_stateMachine.IdleState);
        }

        protected void UpdateMove()
        {
            _moveInputVector = _stateMachine.MovementInput.Move * _stateMachine.MovementSettings.Walk.Speed;

            if (_moveInputVector != _currentMoveInputVector)
            {
                if (_moveInputVector.magnitude == 0) _currentMoveInputVector = Vector2.SmoothDamp(_currentMoveInputVector, _moveInputVector, ref _smoothVelocity, _stateMachine.MovementSettings.Walk.Deceleration);
                else _currentMoveInputVector = Vector2.SmoothDamp(_currentMoveInputVector, _moveInputVector, ref _smoothVelocity, _stateMachine.MovementSettings.Walk.Acceleration);
            }

            var moveVector = GetMoveVectorFromInput(_currentMoveInputVector);

            if (_stateMachine.ActorGroundProbe.GroundAngleFromSphere != 0) moveVector = AdjustMoveToGroundAngle(moveVector);

            moveVector.y -= _stateMachine.MovementSettings.Walk.DownForce;
            
            _stateMachine.LocalMoveVectors = moveVector;
            _stateMachine.Controller.Move(moveVector * Time.deltaTime);
        }

        protected Vector3 GetMoveVectorFromInput(Vector2 moveInput)
        {
            var moveVector = Vector3.zero;
            moveVector += _stateMachine.transform.forward * moveInput.y;
            moveVector += _stateMachine.transform.right * moveInput.x;
            
            return moveVector;
        }

        protected Vector3 AdjustMoveToGroundAngle(Vector3 move)
        {
            return Vector3.ProjectOnPlane(move, _stateMachine.ActorGroundProbe.GroundNormalFromSphere);
        }
    }
}