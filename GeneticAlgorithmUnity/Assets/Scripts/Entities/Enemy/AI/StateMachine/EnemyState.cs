using UnityEngine;

namespace Game.Entities.AI
{
    public abstract class EnemyState
    {
        protected EnemyStateMachine _stateMachine;

        public EnemyState(EnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract void StateStart();
        public abstract void StateUpdate();
        public abstract void StateFixedUpdate();
        public abstract void StateFinish();
    } 
}
