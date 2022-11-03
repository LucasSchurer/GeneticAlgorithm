using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Entities.AI
{
    public class StationaryBehaviour : AIBehaviour
    {
        private MovementController _movementController;

        public override BehaviourType GetBehaviourType() { return BehaviourType.Stationary; }

        protected override void Awake()
        {
            base.Awake();

            _movementController = GetComponent<MovementController>();
        }

        private void FixedUpdate()
        {
            if (_movementController != null)
            {
                _movementController.Rotate(DirectionToPlayer);
            }

            GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, new EntityEventContext() { owner = gameObject });
        }
    } 
}
