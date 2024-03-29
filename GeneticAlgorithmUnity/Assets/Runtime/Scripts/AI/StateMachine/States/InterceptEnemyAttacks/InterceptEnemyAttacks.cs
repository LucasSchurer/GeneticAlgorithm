using UnityEngine;

namespace Game.AI.States
{
    public class InterceptEnemyAttacks : State
    {
        private StateData _data;


        public InterceptEnemyAttacks(StateMachine stateMachine, StateData data) : base(stateMachine, data)
        {
            _data = data;
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