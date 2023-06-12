using UnityEngine;

namespace Game.AI
{
    public class DefaultSupportState : State
    {
        private DefaultSupportStateData _data;

        public DefaultSupportState(StateMachine stateMachine, DefaultSupportStateData data) : base(stateMachine, data)
        {
            _data = data;
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }

        public override void StateStart()
        {
            _stateMachine.ChangeCurrentState(_data.GetTransitionState(_stateMachine, DefaultSupportStateData.Action.Start));
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