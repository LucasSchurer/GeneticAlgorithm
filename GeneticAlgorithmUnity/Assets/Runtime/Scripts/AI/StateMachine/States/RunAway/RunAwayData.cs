using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/RunAwayData")]
    public class RunAwayData : StateData
    {
        private StateData _data;


        public override State GetState(StateMachine stateMachine)
        {
            return new RunAway(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }
    } 
}