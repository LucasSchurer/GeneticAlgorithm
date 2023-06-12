using Game.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public abstract class State
    {
        protected StateMachine _stateMachine;

        private StateData _stateDataDebug;
        public StateData StateDataDebug => _stateDataDebug;

        public State(StateMachine stateMachine, StateData data)
        {
            _stateMachine = stateMachine;
            _stateDataDebug = data;
        }

        public abstract void StateStart();
        public abstract void StateUpdate();
        public abstract void StateFixedUpdate();
        public abstract void StateFinish();
        public abstract Vector3 GetLookDirection();
    }
}
