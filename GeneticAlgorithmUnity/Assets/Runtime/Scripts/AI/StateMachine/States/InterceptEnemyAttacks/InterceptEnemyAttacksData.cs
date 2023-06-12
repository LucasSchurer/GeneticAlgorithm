using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(menuName = Constants.StateDataMenuName + "/InterceptEnemyAttacks")]
    public class InterceptEnemyAttacksData : StateData
    {
        public override State GetState(StateMachine stateMachine)
        {
            return new InterceptEnemyAttacks(stateMachine, this);
        }
    } 
}