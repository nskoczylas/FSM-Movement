using UnityEngine;

namespace FSM
{
    public class StateMachine : MonoBehaviour
    {
        public State CurrentState => _currentState;
        protected State _currentState;

        protected void Update()
        {
            _currentState.CheckSwitchState();
            _currentState.OnUpdate();
        }

        protected void FixedUpdate()
        {
            _currentState.OnFixedUpdate();
        }

        public void SwitchState(State newState)
        {
            _currentState.OnExit();

            _currentState = newState;
            _currentState.OnEnter();
        }
    }
}