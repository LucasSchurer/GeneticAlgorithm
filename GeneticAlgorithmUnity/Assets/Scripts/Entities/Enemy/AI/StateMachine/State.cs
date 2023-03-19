using UnityEngine;

namespace Game.Entities.AI
{
    public abstract class State
    {
        protected StateMachine _stateMachine;
        protected StateMachineData _data;

        public abstract StateType GetStateType();

        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract void StateStart();
        public abstract void StateUpdate();
        public abstract void StateFixedUpdate();
        public abstract void StateFinish();
    } 
}
