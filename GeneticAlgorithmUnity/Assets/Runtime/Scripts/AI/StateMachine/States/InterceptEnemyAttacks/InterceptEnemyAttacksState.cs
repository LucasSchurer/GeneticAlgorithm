using UnityEngine;

namespace Game.AI
{
    public class InterceptEnemyAttacksState : State
    {
        private StateData _data;


        public InterceptEnemyAttacksState(StateMachine stateMachine, StateData data) : base(stateMachine, data)
        {
            _data = data;
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }

        public override void StateStart()
        {
            
        }

        public override void StateUpdate()
        {
            
        }

        public override void StateFixedUpdate()
        {
            
        }

        public override void StateFinish()
        {
            
        }

        public override Vector3 GetLookDirection()
        {
            return Vector3.zero;
        }
    } 
}