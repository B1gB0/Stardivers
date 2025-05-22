using System;
using System.Collections.Generic;

namespace Project.Scripts.UI.StateMachine
{
    public class UIStateMachine
    {
        private readonly Dictionary<Type, UIState> _states = new ();

        private UIState _currentState;

        public void EnterIn<T>() where T : UIState
        {
            var type = typeof(T);

            if (_currentState != null && _currentState.GetType() == type)
            {
                return;
            }

            if (!_states.TryGetValue(type, out var newState)) return;
            
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void AddState(UIState state)
        {
            var type = state.GetType();

            if (_states.ContainsKey(type) == false)
            {
                _states.TryAdd(type, state);
            }
        }

        public void RemoveState<T>() where T : UIState
        {
            var type = typeof(T);
            
            if (_states.ContainsKey(type))
            {
                _states.Remove(type);
            }
        }
    }
}