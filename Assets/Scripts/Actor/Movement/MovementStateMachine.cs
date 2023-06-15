using System;
using Actor.Movement.Data;
using Actor.Movement.States.Airborne;
using Actor.Movement.States.Grounded;
using FSM;
using Player;
using UnityEngine;

namespace Actor.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(GroundProbe))]
    public class MovementStateMachine : StateMachine
    {
        #region References

        public CharacterController Controller => _controller;
        private CharacterController _controller;

        public GroundProbe ActorGroundProbe => _groundProbe;
        private GroundProbe _groundProbe;

        public Transform CameraRig => _cameraRig;
        [SerializeField] private Transform _cameraRig;

        public IMovementInput MovementInput => _movementInput;
        private IMovementInput _movementInput;

        #endregion
        
        [HideInInspector] public float CameraPitch;

        [HideInInspector] public Vector2 CurrentMoveInput;
        [HideInInspector] public Vector2 TargetMoveInput;
        [HideInInspector] public Vector3 LocalMoveVectors;

        public MovementData Data => _movementData;
        [SerializeField] private MovementData _movementData;

        #region States

        public Idle IdleState => _idle;
        private Idle _idle;

        public Walk WalkState => _walk;
        private Walk _walk;

        public Run RunState => _run;
        private Run _run;

        public SlopeSlide SlopeSlideState => _slopeSlide;
        private SlopeSlide _slopeSlide;

        public Fall FallState => _fall;
        private Fall _fall;

        #endregion
        
        private void Awake()
        {
            _controller = gameObject.GetComponent<CharacterController>();
            _groundProbe = gameObject.GetComponent<GroundProbe>();

            CreateMovementInputFromPlayer();

            CameraPitch = CameraRig.localRotation.x;
            LocalMoveVectors = Vector3.zero;

            CreateStates();
            SwitchState(_idle);
        }

        private void CreateStates()
        {
            _idle = new Idle(this);
            _walk = new Walk(this);
            _run = new Run(this);

            _slopeSlide = new SlopeSlide(this);

            _fall = new Fall(this);
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
