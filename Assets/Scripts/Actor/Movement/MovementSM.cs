using System;
using Actor.Movement.Data;
using Actor.Movement.States.Grounded;
using FSM;
using Player;
using UnityEngine;

namespace Actor.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementSM : StateMachine
    {
        #region References

        public CharacterController Controller => _controller;
        [SerializeField] private CharacterController _controller;

        public Transform CameraRig => _cameraRig;
        [SerializeField] private Transform _cameraRig;

        public IMovementInput MovementInput => _movementInput;
        private IMovementInput _movementInput;

        #endregion

        public MovementData MovementSettings => _movementData;
        [SerializeField] private MovementData _movementData;

        #region States

        public Idle IdleState => _idle;
        private Idle _idle;

        #endregion
        
        private void Awake()
        {
            _controller = gameObject.GetComponent<CharacterController>();
            
            CreateMovementInputFromPlayer();
            
            CreateStates();
            SwitchState(_idle);
        }

        private void CreateStates()
        {
            _idle = new Idle(this);
        }

        private void CreateMovementInputFromPlayer()
        {
            var newPlayerInputAdapter = new GameObject("PlayerInputMovementAdapter");
            _movementInput = newPlayerInputAdapter.AddComponent<PlayerInputAdapter>();
        }

        private void RemoveMovementInputFromPlayer()
        {
            if (_movementInput.GetType() != typeof(PlayerInputAdapter)) return;

            var inputAdapter = (PlayerInputAdapter)_movementInput;
            Destroy(inputAdapter.gameObject);

            _movementInput = null;
        }
    }
}
