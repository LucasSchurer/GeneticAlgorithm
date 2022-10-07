using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    protected Transform _playerTransform;
    protected Vector3 DirectionToPlayer => (_playerTransform.position - transform.position).normalized;
    protected BehaviourType _behaviourType;

    protected virtual void Awake()
    {
        _behaviourType = GetBehaviourType();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public abstract BehaviourType GetBehaviourType();

    public static void AddAIBehaviour(GameObject go, BehaviourType type)
    {
        switch (type)
        {
            case BehaviourType.Aggressive:
                go.AddComponent<AIAggressiveBehaviour>();
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
