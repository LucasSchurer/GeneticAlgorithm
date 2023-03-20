using Game.Entities.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class BehaviourManager : Singleton<BehaviourManager>
    {
        [SerializeField]
        private Behaviour[] _behavioursArray;

        private Dictionary<BehaviourType, Behaviour> _behaviours;

        protected override void SingletonAwake()
        {
            _behaviours = new Dictionary<BehaviourType, Behaviour>();

            foreach (Behaviour b in _behavioursArray)
            {
                _behaviours.TryAdd(b.Type, b);
            }
        }

        public StateMachineData GetBehaviourStateMachineData(BehaviourType type)
        {
            if (_behaviours.TryGetValue(type, out Behaviour behaviour))
            {
                return behaviour.StateMachineData;
            }

            Debug.LogError("Behaviour Type is not contained in behaviour list!");

            return null;
        }

        [System.Serializable]
        private struct Behaviour
        {
            [SerializeField]
            private BehaviourType _type;
            [SerializeField]
            private StateMachineData _stateMachineData;
            public BehaviourType Type => _type;
            public StateMachineData StateMachineData => _stateMachineData;
        }
    } 
}
