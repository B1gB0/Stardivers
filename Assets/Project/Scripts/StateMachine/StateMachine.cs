using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.CommonModules.StateMachineModule
{
    public class StateMachine
    {
        private Dictionary<Type, State> _states = new Dictionary<Type, State>();
        
        public State CurrentState { get; private set; }

        public void AddState(State state)
        {
            _states.Add(state.GetType(), state);
        }

        public void SetState<T>() where T : State
        {
            var type = typeof(T);

            if (CurrentState.GetType() == type)
            {
                return;
            }

            if (_states.TryGetValue(type, out var newState))
            {
                CurrentState?.Exit();

                CurrentState = newState;
                
                CurrentState.Enter();
            }
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}

