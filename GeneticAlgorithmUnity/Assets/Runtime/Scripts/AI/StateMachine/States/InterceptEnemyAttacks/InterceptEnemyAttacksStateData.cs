using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/InterceptEnemyAttacksStateData")]
    public class InterceptEnemyAttacksStateData : StateData
    {
        public override State GetState(StateMachine stateMachine)
        {
            return new InterceptEnemyAttacksState(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.None;
        }
    } 
}