using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source.CommonModules.StateMachineModule
{
    public abstract class State
    {
        protected readonly StateMachine StateMachine;

        public State(StateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
    }
}

