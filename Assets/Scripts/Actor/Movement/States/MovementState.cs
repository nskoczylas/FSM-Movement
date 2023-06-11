using FSM;
using UnityEngine;

namespace Actor.Movement.States
{
    public abstract class MovementState : State
    {
        protected new MovementSM _stateMachine;
        
        protected Vector2 _currentMoveInput
        {
            get => _stateMachine.CurrentMoveInput;
            set => _stateMachine.CurrentMoveInput = value;
        }
        
        protected Vector2 _targetMoveInput
        {
            get => _stateMachine.TargetMoveInput;
            set => _stateMachine.TargetMoveInput = value;
        }

        protected Vector2 _inputSmoothingVelocity;
        
        public MovementState(StateMachine stateMachine) : base(stateMachine)
        {
            _stateMachine = (MovementSM)stateMachine;
        }

        public override void OnEnter()
        {
            Debug.Log($"Entered: {this.GetType()}");
        }

        public override void OnUpdate()
        {
            UpdateView();
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnExit()
        {
        }

        protected virtual void UpdateView()
        {
            var actorRotationInput = _stateMachine.MovementInput.View.x * _stateMachine.Data.Idle.CameraSensitivity.x;
            var cameraPitchInput = _stateMachine.MovementInput.View.y * _stateMachine.Data.Idle.CameraSensitivity.y;

            _stateMachine.transform.rotation *= Quaternion.Euler(0, actorRotationInput, 0);

            _stateMachine.CameraPitch -= cameraPitchInput;
            _stateMachine.CameraPitch = Mathf.Clamp(_stateMachine.CameraPitch, _stateMachine.Data.Idle.CameraClamp.x, _stateMachine.Data.Idle.CameraClamp.y);
            _stateMachine.CameraRig.localRotation = Quaternion.Euler(_stateMachine.CameraPitch, 0, 0);
        }
        
        protected virtual bool ShouldBeFalling(float allowedTimeSinceGrounded)
        {
            if (!_stateMachine.ActorGroundProbe.IsGrounded && _stateMachine.ActorGroundProbe.TimeSinceGrounded > allowedTimeSinceGrounded) return true;
            return false;
        }

        protected virtual bool ShouldBeSliding()
        {
            if (!(_stateMachine.Controller.slopeLimit < _stateMachine.ActorGroundProbe.GroundAngleFromRay)) return false;
            if (_stateMachine.Controller.slopeLimit < _stateMachine.ActorGroundProbe.GroundAngleFromSphere) return true;
            return false;
        }
        
        protected void UpdateTargetMoveInputVector(float speed)
        {
            _targetMoveInput = _stateMachine.MovementInput.Move * speed;
        }
        
        protected virtual void UpdateCurrentMoveInputVector(float accelerationTime, float decelerationTime)
        {
            if (_targetMoveInput == _currentMoveInput) return;

            if (_targetMoveInput.magnitude == 0)
            {
                _currentMoveInput = _currentMoveInput.magnitude < 0.01 
                    ? Vector2.zero
                    : Vector2.SmoothDamp(_currentMoveInput, _targetMoveInput, ref _inputSmoothingVelocity, decelerationTime);
            }
            _currentMoveInput = Vector2.SmoothDamp(_currentMoveInput, _targetMoveInput, ref _inputSmoothingVelocity, accelerationTime);
        }
    }
}