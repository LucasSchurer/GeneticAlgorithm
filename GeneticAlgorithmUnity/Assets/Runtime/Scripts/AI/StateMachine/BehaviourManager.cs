using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
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

        public BehaviourType GetRandomBehaviourType()
        {
            return _behavioursArray[Random.Range(0, _behavioursArray.Length)].Type;
        }

        public BehaviourType GetRandomBehaviourType(System.Random rand)
        {
            int index = rand.Next(0, _behavioursArray.Length);

            return _behavioursArray[index].Type;
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
