using System;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Player.PlayerInput;

namespace Actor.Movement
{
    public class PlayerInputAdapter : MonoBehaviour, IMovementInput
    {
        public Vector2 View => _view;
        public Vector2 Move => _move;
        public Action<ActionStage> Jump => _jump;
        public Action<ActionStage> Sprint => _sprint;
        public Action<ActionStage> Crouch => _crouch;

        private PlayerInput _playerInput;
        private bool _isEnabled = false;
        
        private Vector2 _view;
        private Vector2 _move;
        private Action<ActionStage> _jump;
        private Action<ActionStage> _sprint;
        private Action<ActionStage> _crouch;

        private void Awake()
        {
            _playerInput = PlayerInput.Instance;
            Enable();
        }

        private void Update()
        {
            UpdateVectors();
        }

        private void OnDestroy()
        {
            Disable();
        }

        private void UpdateVectors()
        {
            if (!_isEnabled) return;
            
            _view = _playerInput.GameActions.View.ReadValue<Vector2>();
            _move = _playerInput.GameActions.Move.ReadValue<Vector2>();
        }

        private void Enable()
        {
            _playerInput.GameActions.Jump.started += OnJump;
            _playerInput.GameActions.Jump.canceled += OnJump;
            _playerInput.GameActions.Sprint.started += OnSprint;
            _playerInput.GameActions.Sprint.canceled += OnSprint;
            _playerInput.GameActions.Crouch.started += OnCrouch;
            _playerInput.GameActions.Crouch.canceled += OnCrouch;
            
            _isEnabled = true;
        }

        private void Disable()
        {
            _playerInput.GameActions.Jump.started -= OnJump;
            _playerInput.GameActions.Jump.canceled -= OnJump;
            _playerInput.GameActions.Sprint.started -= OnSprint;
            _playerInput.GameActions.Sprint.canceled -= OnSprint;
            _playerInput.GameActions.Crouch.started -= OnCrouch;
            _playerInput.GameActions.Crouch.canceled -= OnCrouch;
            
            _isEnabled = false;
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            InvokeActionWithStage(_jump, ctx);
        }

        private void OnSprint(InputAction.CallbackContext ctx)
        {
            InvokeActionWithStage(_sprint, ctx);
        }

        private void OnCrouch(InputAction.CallbackContext ctx)
        {
            InvokeActionWithStage(_crouch, ctx);
        }

        private void InvokeActionWithStage(Action<ActionStage> inputAction, InputAction.CallbackContext ctx)
        {
            if (ctx.started) inputAction?.Invoke(ActionStage.Pressed);
            if (ctx.canceled) inputAction?.Invoke(ActionStage.Released);
        }
    }
}