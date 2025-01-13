using System.Collections.Generic;
using System;
using Project.Scripts.EnemyAnimation.States;
using UnityEngine;

namespace Project.Scripts.EnemyAnimation
{
    public class AnimatedStateMachine
    {
        private readonly Dictionary<Type, AnimatedState> _states = new();
        
        public AnimatedStateMachine(Animator animator)
        {
            AnimationNamesBase animationBase = new ();

            AddState(new IdleState(animator, animationBase));
            AddState(new GetGunState(animator, animationBase));
            AddState(new MoveState(animator, animationBase));
            AddState(new AttackState(animator, animationBase));
        }

        private AnimatedState _currentState;

        public void EnterIn<T>() where T : AnimatedState
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
        
        private void AddState(AnimatedState state)
        {
            var type = state.GetType();

            if (_states.ContainsKey(type) == false)
            {
                _states.TryAdd(type, state);
            }
        }
    }
}