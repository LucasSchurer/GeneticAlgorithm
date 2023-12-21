using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/RunAway")]
    public class RunAwayData : StateData
    {
        public override State GetState(StateMachine stateMachine)
        {
            return new RunAway(stateMachine, this);
        }
    } 
}