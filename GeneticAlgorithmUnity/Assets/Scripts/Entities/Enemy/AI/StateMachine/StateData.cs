using UnityEngine;

namespace Game.Entities.AI
{
    [CreateAssetMenu(fileName = "StateData", menuName = "AI/StateMachine/StateData")]
    public class StateData : ScriptableObject
    {
        [SerializeField]
        private StateType _stateType;

        public StateType StateType => _stateType;
    } 
}
