using System;
using System.Collections.Generic;

namespace Project.Scripts.Game.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, GameState> _states = new ();
        private GameState _currentState;

        public void EnterIn<T>() where T : GameState
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
        
        public void AddState(GameState state)
        {
            var type = state.GetType();

            _states.TryAdd(type, state);
        }

        public void RemoveState(GameState state)
        {
            var type = state.GetType();
            
            if (_states.ContainsKey(type))
            {
                _states.Remove(type);
            }
        }
    }
}