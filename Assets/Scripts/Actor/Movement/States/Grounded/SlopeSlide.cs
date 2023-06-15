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

        public override void OnEnter()
        {
            base.OnEnter();
            _currentSlideSpeed = _stateMachine.LocalMoveVectors.magnitude;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateSlideVelocity();
        }

        public override void CheckSwitchState()
        {
            if (ShouldBeFalling(0.25f))
            {
                _stateMachine.SwitchState(_stateMachine.FallState);
                return;
            }
            
            if (!IsOnSteepTerrain())
            {
                if (_currentSlideSpeed < 1.5f) _stateMachine.SwitchState(_stateMachine.IdleState); 
            }
        }

        private bool IsOnSteepTerrain()
        {
            return _stateMachine.Controller.slopeLimit < _stateMachine.ActorGroundProbe.GroundAngleFromSphere;
        }

        private void UpdateSlideVelocity()
        {
            if (IsOnSteepTerrain())
            {
                var maxSlideSpeed = _stateMachine.Data.SlopeSlide.MaxSlideSpeed;
                var accelerationTime = _stateMachine.Data.SlopeSlide.AccelerationTime;
                _currentSlideSpeed = Mathf.SmoothDamp(_currentSlideSpeed, maxSlideSpeed, ref _smoothVelocity, accelerationTime);
                
                var slopeVector = Vector3.Cross(Vector3.up, _stateMachine.ActorGroundProbe.GroundNormalFromSphere);
                slopeVector = Vector3.Cross(slopeVector, _stateMachine.ActorGroundProbe.GroundNormalFromSphere);

                _stateMachine.LocalMoveVectors = slopeVector;
            }
            else
            {
                var decelerationTime = _stateMachine.Data.SlopeSlide.DecelerationTime;
                _currentSlideSpeed = Mathf.SmoothDamp(_currentSlideSpeed, 0, ref _smoothVelocity, decelerationTime);
            }

            _stateMachine.Controller.Move(_stateMachine.LocalMoveVectors * (_currentSlideSpeed * Time.deltaTime));
        }
    }
}