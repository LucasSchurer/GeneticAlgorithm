using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAggressiveBehaviour : AIBehaviour
{
    private MovementController _movementController;

    public override BehaviourType GetBehaviourType() { return BehaviourType.Aggressive; }

    protected override void Awake()
    {
        base.Awake();

        _movementController = GetComponent<MovementController>();
    }

    private void FixedUpdate()
    {
        if (_movementController != null)
        {
            _movementController.Move(DirectionToPlayer);
            _movementController.Rotate(DirectionToPlayer);
        }
    }
}
