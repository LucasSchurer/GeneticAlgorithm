using UnityEngine;

namespace Game.Entities.AI
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
    } 
}
