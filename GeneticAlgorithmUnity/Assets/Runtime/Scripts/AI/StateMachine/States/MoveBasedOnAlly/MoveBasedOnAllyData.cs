using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/MoveBasedOnAlly")]
    public class MoveBasedOnAllyData : StateData
    {
        public override State GetState(StateMachine stateMachine)
        {
            return new MoveBasedOnAlly(stateMachine, this);
        }
    } 
}