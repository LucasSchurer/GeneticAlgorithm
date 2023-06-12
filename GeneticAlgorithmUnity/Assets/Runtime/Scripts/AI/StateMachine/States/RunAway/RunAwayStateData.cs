using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/RunAwayStateData")]
    public class RunAwayStateData : StateData
    {
        private StateData _data;


        public override State GetState(StateMachine stateMachine)
        {
            return new RunAwayState(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }
    } 
}