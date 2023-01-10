using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public abstract class AIBehaviour : MonoBehaviour
    {
        protected EntityEventController _eventController;
        protected StatusEffectsController _statusEffectsController;
        protected Transform _playerTransform;
        protected Vector3 DirectionToPlayer => _playerTransform ? (_playerTransform.position - transform.position).normalized : Vector3.zero;
        protected BehaviourType _behaviourType;

        protected virtual void Awake()
        {
            _behaviourType = GetBehaviourType();
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _eventController = GetComponent<EntityEventController>();
        }

        public abstract BehaviourType GetBehaviourType();

        public static void AddAIBehaviour(GameObject go, BehaviourType type)
        {
            switch (type)
            {
                case BehaviourType.Aggressive:
                    go.AddComponent<AggressiveBehaviour>();
                    break;
                case BehaviourType.Cautious:
                    break;
                case BehaviourType.Stationary:
                    break;
                case BehaviourType.Count:
                    break;
            }
        }
    } 
}
