using Actor.Movement.Data.Grounded;
using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class Walk : Idle
    {
        protected Vector2 _moveInputVector
        {
            get { return _stateMachine.TargetMoveInput; }
            set { _stateMachine.TargetMoveInput = value; }
        }
        protected Vector2 _currentMoveInputVector
        {
            get { return _stateMachine.CurrentMoveInput; }
            set { _stateMachine.CurrentMoveInput = value; }
        }

        protected Vector2 _smoothVelocity;
        protected Vector3 _moveVector;

        protected bool _tiesToRun = false;
        
        public Walk(StateMachine stateMachine) : base(stateMachine)
        {
            _stateMachine.MovementInput.Sprint += OnSprint;
        }

        ~Walk()
        {
            _stateMachine.MovementInput.Sprint -= OnSprint;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateMove();
        }

        public override void CheckSwitchState()
        {
            CheckIsFalling(0.1f);
            CheckIsSlidingDownSlope();

            if (_tiesToRun && _moveInputVector.magnitude != 0)
            {
                _stateMachine.SwitchState(_stateMachine.RunState);
                return;
            }
            
            if (_moveInputVector.magnitude == 0 && _currentMoveInputVector.magnitude == 0) _stateMachine.SwitchState(_stateMachine.IdleState);
        }

        protected virtual void UpdateMove()
        {
            _moveInputVector = GetMoveInputVector(_stateMachine.MovementSettings.Walk);
            _currentMoveInputVector = GetCurrentMoveInputVector(_stateMachine.MovementSettings.Walk);
            _moveVector = GetMoveVectorFromInput(_currentMoveInputVector);
            
            if (ShouldCorrectToSlope()) _moveVector = AdjustMoveToGroundAngle(_moveVector);

            _moveVector.y -= _stateMachine.MovementSettings.Walk.DownForce;
            Move(_moveVector);
        }

        protected virtual void OnSprint(ActionStage stage)
        {
            if (stage == ActionStage.Pressed)
            {
                _tiesToRun = true;
                return;
            }
            _tiesToRun = false;
        }

        protected Vector2 GetMoveInputVector(WalkData data)
        {
            return _stateMachine.MovementInput.Move * data.Speed;
        }

        protected Vector2 GetCurrentMoveInputVector(WalkData data)
        {
            if (_moveInputVector == _currentMoveInputVector) return _currentMoveInputVector;

            if (_moveInputVector.magnitude == 0)
            {
                return _currentMoveInputVector.magnitude < 0.01 
                    ? Vector2.zero
                    : Vector2.SmoothDamp(_currentMoveInputVector, _moveInputVector, ref _smoothVelocity, data.Deceleration);
            }
            return Vector2.SmoothDamp(_currentMoveInputVector, _moveInputVector, ref _smoothVelocity, data.Acceleration);
        }

        protected Vector3 GetMoveVectorFromInput(Vector2 moveInput)
        {
            var moveVector = Vector3.zero;
            moveVector += _stateMachine.transform.forward * moveInput.y;
            moveVector += _stateMachine.transform.right * moveInput.x;
            
            return moveVector;
        }

        protected bool ShouldCorrectToSlope()
        {
            if (_stateMachine.ActorGroundProbe.GroundAngleFromSphere == 0) return false;
            if (_stateMachine.ActorGroundProbe.DistanceToGround > 0.9f) return false;
            return true;
        }

        protected Vector3 AdjustMoveToGroundAngle(Vector3 move)
        {
            return Vector3.ProjectOnPlane(move, _stateMachine.ActorGroundProbe.GroundNormalFromSphere);
        }

        protected void Move(Vector3 move)
        {
            _stateMachine.LocalMoveVectors = move;
            _stateMachine.Controller.Move(move * Time.deltaTime);
        }
    }
}