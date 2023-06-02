using System;
using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        #region Singleton

        public static PlayerInput Instance
        {
            get
            {
                if (ReferenceEquals(_instance, null))
                {
                    var newPlayerInput = new GameObject("PlayerInput");
                    _instance = newPlayerInput.AddComponent<PlayerInput>();
                }

                return _instance;
            }
        }
        private static PlayerInput _instance;

        #endregion

        public Controls.GameActions GameActions => _controls.Game;
        private Controls _controls;

        private void Awake()
        {
            _instance ??= this;
            if (!ReferenceEquals(_instance, null) && !ReferenceEquals(_instance, this)) Destroy(gameObject);
            
            InitializeControls();
        }

        private void InitializeControls()
        {
            _controls = new Controls();
            EnableControls();
        }

        private void EnableControls()
        {
            _controls.Enable();
        }

        private void DisableControls()
        {
            _controls.Disable();
        }
    }
}