using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
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
    } 
}
