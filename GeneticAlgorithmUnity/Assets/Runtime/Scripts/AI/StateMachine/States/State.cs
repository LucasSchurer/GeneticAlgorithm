using Game.Events;
using UnityEngine;

namespace Game.AI
{
    public abstract class State
    {
        protected StateMachine _stateMachine;

        public abstract StateType GetStateType();

        public State(StateMachine stateMachine, StateData data)
        {
            _stateMachine = stateMachine;
        }

        public abstract void StateStart();
        public abstract void StateUpdate();
        public abstract void StateFixedUpdate();
        public abstract void StateFinish();
        public abstract Vector3 GetLookDirection();
    }
}
