using Actor.Movement.States.Grounded;
using FSM;
using UnityEngine;

namespace Actor.Movement.States.Airborne
{
    public class Fall : Idle
    {
        protected float _fallSpeed = 0f;
        
        public Fall(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            SetFallSpeed();
            SetMoveVectors();
            ApplyMovement();
        }

        public override void CheckSwitchState()
        {
            if (!_stateMachine.ActorGroundProbe.IsGrounded) return;
            _stateMachine.SwitchState(_stateMachine.IdleState);
        }

        public override void OnExit()
        {
            base.OnExit();
            _stateMachine.LocalMoveVectors = Vector3.zero;
            _fallSpeed = 0;
        }

        private void SetFallSpeed()
        {
            if (_fallSpeed == 0 && _stateMachine.MovementSettings.Fall.InitialGravityKick != 0)
            {
                _fallSpeed += _stateMachine.MovementSettings.Fall.InitialGravityKick;
                return;
            }

            _fallSpeed += _stateMachine.MovementSettings.Fall.Gravity * Time.deltaTime;
        }

        private void SetMoveVectors()
        {
            if (_stateMachine.MovementSettings.Fall.DecelerationForce == 0) return;
            if (_stateMachine.LocalMoveVectors.x == 0 && _stateMachine.LocalMoveVectors.z == 0) return;

            _stateMachine.LocalMoveVectors.x = ApplyDeceleration(_stateMachine.LocalMoveVectors.x);
            _stateMachine.LocalMoveVectors.z = ApplyDeceleration(_stateMachine.LocalMoveVectors.z);
        }

        private float ApplyDeceleration(float vector)
        {
            if (vector == 0) return 0f;
            
            if (vector > 0)
            {
                vector -= _stateMachine.MovementSettings.Fall.DecelerationForce * Time.deltaTime;
                vector = Mathf.Round(vector * 100) * 0.01f;
                if (vector <= 0) return 0;
            }

            if (vector < 0)
            {
                vector += _stateMachine.MovementSettings.Fall.DecelerationForce * Time.deltaTime;
                vector = Mathf.Round(vector * 100) * 0.01f;
                if (vector >= 0) return 0;
            }

            return vector;
        }

        private void ApplyMovement()
        {
            var movement = _stateMachine.LocalMoveVectors;
            movement.y = _fallSpeed * -1;
            
            _stateMachine.Controller.Move(movement * Time.deltaTime);
        }
    }
}