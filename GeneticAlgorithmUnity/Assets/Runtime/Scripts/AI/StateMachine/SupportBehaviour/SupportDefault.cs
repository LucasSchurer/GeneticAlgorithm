using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public class SupportDefault : State
    {
        private SupportDefaultData _data;
        private HashSet<SupportDefaultData.Action> _blockedActions;

        public SupportDefault(StateMachine stateMachine, SupportDefaultData data) : base(stateMachine, data)
        {
            _data = data;
            _blockedActions = new HashSet<SupportDefaultData.Action>();
        }

        public override void StateStart()
        {
            State nextState = null;

            if (!_blockedActions.Contains(SupportDefaultData.Action.CloseToDeath) && _stateMachine.Health.CurrentValue / _stateMachine.Health.MaxValue <= _data.CloseToDeathHealthThreshold)
            {
                nextState = _data.GetTransitionState(_stateMachine, _blockedActions, SupportDefaultData.Action.CloseToDeath);
            } 

            if (nextState != null)
            {
                _stateMachine.ChangeCurrentState(nextState);
            } else
            {
                _stateMachine.ChangeCurrentState(_data.GetTransitionState(_stateMachine, _blockedActions, SupportDefaultData.Action.Start));
            }
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