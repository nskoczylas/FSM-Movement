using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class Idle : MovementState
    {
        protected float _cameraPitch = 0f;
        
        public Idle(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Entered IDLE STATE");

            _cameraPitch = _stateMachine.CameraRig.rotation.x;
        }

        public override void OnUpdate()
        {
            UpdateView();
        }

        public override void OnFixedUpdate()
        {
        }

        public override void CheckSwitchState()
        {
            if (_stateMachine.MovementInput.Move.magnitude != 0)
            {
                Debug.Log("Switch state to moving.");
            }
        }

        public override void OnExit()
        {
        }

        protected void UpdateView()
        {
            var actorRotationInput = _stateMachine.MovementInput.View.x * _stateMachine.MovementSettings.Idle.CameraSensitivity.x;
            var cameraPitchInput = _stateMachine.MovementInput.View.y * _stateMachine.MovementSettings.Idle.CameraSensitivity.y;

            _stateMachine.transform.rotation *= Quaternion.Euler(0, actorRotationInput, 0);

            _cameraPitch -= cameraPitchInput;
            _cameraPitch = Mathf.Clamp(_cameraPitch, _stateMachine.MovementSettings.Idle.CameraClamp.x, _stateMachine.MovementSettings.Idle.CameraClamp.y);
            _stateMachine.CameraRig.localRotation = Quaternion.Euler(_cameraPitch, 0, 0);
        }
    }
}