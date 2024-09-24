using System;
using System.Collections.Generic;

namespace Project.Scripts.UI.StateMachine
{
    public class UIStateMachine
    {
        private readonly Dictionary<Type, IUIState> _states = new ();
        private IUIState _currentState;

        public void EnterIn<T>() where T : IUIState
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
        
        public void AddState(IUIState state)
        {
            var type = state.GetType();

            _states.TryAdd(type, state);
        }
    }
}