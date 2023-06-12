using UnityEngine;

namespace Game.AI.States
{
    public class FindAllyState : State
    {
        private StateData _data;


        public FindAllyState(StateMachine stateMachine, StateData data) : base(stateMachine, data)
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