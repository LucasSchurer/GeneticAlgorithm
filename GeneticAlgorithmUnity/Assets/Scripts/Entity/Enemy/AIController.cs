using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private BehaviourType _behaviourType;
    private AIBehaviour _behaviour;

    private Vector3 _direction;
    private Transform _playerPosition;

    private MovementController _movementController;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
        _playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _direction = (_playerPosition.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        _movementController?.Move(_direction);
        _movementController?.Rotate(_direction);
    }

    private AIBehaviour GetAIBehaviour()
    {
        switch (_behaviourType)
        {
            case BehaviourType.Aggressive:
                return null;
        }

        return null;
    }
}
