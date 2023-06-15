using Actor.Movement.Data.Grounded;
using FSM;
using UnityEngine;

namespace Actor.Movement.States.Grounded
{
    public class Walk : Idle
    {
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

            if (_tiesToRun && _targetMoveInput.magnitude != 0)
            {
                _stateMachine.SwitchState(_stateMachine.RunState);
                return;
            }

            if (_targetMoveInput.magnitude == 0 && _currentMoveInput.magnitude == 0)
            {
                _stateMachine.SwitchState(_stateMachine.IdleState);
                return;
            }
        }

        protected virtual void UpdateMove()
        {
            UpdateTargetMoveInputVector(_stateMachine.Data.Walk.Speed);
            UpdateCurrentMoveInputVector(_stateMachine.Data.Walk.Acceleration, _stateMachine.Data.Walk.Deceleration);
            _moveVector = GetMoveVectorFromInput(_currentMoveInput);
            
            if (ShouldCorrectToSlope()) _moveVector = AdjustMoveToGroundAngle(_moveVector);

            _moveVector.y -= _stateMachine.Data.Walk.DownForce;
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

        protected Vector3 GetMoveVectorFromInput(Vector2 moveInput)
        {
            var transform = _stateMachine.transform;
            var moveVector = Vector3.zero;
            
            moveVector += transform.forward * moveInput.y;
            moveVector += transform.right * moveInput.x;
            
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