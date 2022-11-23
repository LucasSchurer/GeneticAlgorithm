using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Entities.AI
{
    public class CautiousBehaviour : AIBehaviour
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
                _movementController.Move(DirectionToPlayer * -1);
                _movementController.Rotate(DirectionToPlayer);
            }

            GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, null);
        }
    } 
}
