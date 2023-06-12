using UnityEngine;

namespace Game.AI.States
{
    public class SupportDefault : State
    {
        private SupportDefaultData _data;

        public SupportDefault(StateMachine stateMachine, SupportDefaultData data) : base(stateMachine, data)
        {
            _data = data;
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }

        public override void StateStart()
        {
            _stateMachine.ChangeCurrentState(_data.GetTransitionState(_stateMachine, SupportDefaultData.Action.Start));
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