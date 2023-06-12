using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/FindAllyStateData")]
    public class FindAllyStateData : StateData
    {
        public override State GetState(StateMachine stateMachine)
        {
            return new FindAllyState(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }
    } 
}