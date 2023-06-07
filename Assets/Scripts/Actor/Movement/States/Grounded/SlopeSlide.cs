using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class SlopeSlide : Idle
    {
        private float _currentSlideSpeed;
        private float _smoothVelocity;
        
        public SlopeSlide(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateSlideVelocity();
        }

        public override void CheckSwitchState()
        {
            CheckIsFalling(0.1f);
            if (!IsOnSteepTerrain()) _stateMachine.SwitchState(_stateMachine.IdleState);
        }

        private bool IsOnSteepTerrain()
        {
            return _stateMachine.Controller.slopeLimit < _stateMachine.ActorGroundProbe.GroundAngleFromSphere;
        }

        private void UpdateSlideVelocity()
        {
            if (IsOnSteepTerrain())
            {
                var maxSlideSpeed = _stateMachine.MovementSettings.SlopeSlide.MaxSlideSpeed;
                var accelerationTime = _stateMachine.MovementSettings.SlopeSlide.AccelerationTime;
                _currentSlideSpeed = Mathf.SmoothDamp(_currentSlideSpeed, maxSlideSpeed, ref _smoothVelocity, accelerationTime);
            }
            else
            {
                var decelerationTime = _stateMachine.MovementSettings.SlopeSlide.DecelerationTime;
                _currentSlideSpeed = Mathf.SmoothDamp(_currentSlideSpeed, 0, ref _smoothVelocity, decelerationTime);
            }
            
            var slopeVector = Vector3.Cross(Vector3.up, _stateMachine.ActorGroundProbe.GroundNormalFromSphere);
            slopeVector = Vector3.Cross(slopeVector, _stateMachine.ActorGroundProbe.GroundNormalFromSphere);

            slopeVector *= _currentSlideSpeed;

            _stateMachine.Controller.Move(slopeVector * Time.deltaTime);
        }
    }
}