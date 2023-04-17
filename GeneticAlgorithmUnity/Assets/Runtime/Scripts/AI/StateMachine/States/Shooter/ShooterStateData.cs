using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(fileName = "ShooterData", menuName = "AI/StateMachine/States/ShooterStateData")]
    public class ShooterStateData : StateData
    {
        [SerializeField]
        private Vector2 _keepDistanceRange;

        public Vector2 KeepDistanceRange => _keepDistanceRange;

        public override State GetState(StateMachine stateMachine)
        {
            return new ShooterState(stateMachine, this);
        }

        public override StateType GetStateType()
        {
            return StateType.Idle;
        }
    } 
}
