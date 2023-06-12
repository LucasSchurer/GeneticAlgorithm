using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/FindAlly")]
    public class FindAllyData : StateData
    {
        public override State GetState(StateMachine stateMachine)
        {
            return new FindAlly(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }
    } 
}