using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    [CreateAssetMenu(fileName = "StateMachineData", menuName = "AI/StateMachine/StateMachineData")]
    public class StateMachineData : ScriptableObject
    {
        [SerializeField]
        private StateData _initialState;
        [SerializeField]
        private StateData _defaultState;

        [SerializeField]
        private List<StateTransition> _transitions;
    } 
}
