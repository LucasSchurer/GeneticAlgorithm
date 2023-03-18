using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System;

namespace Game.Entities.AI
{
    public class StationaryBehaviour : AIBehaviour
    {
        private MovementController _movementController;

        public override BehaviourType GetBehaviourType() { return BehaviourType.Coward; }

        protected override void Awake()
        {
            base.Awake();

            _movementController = GetComponent<MovementController>();
        }

        private void FixedUpdate()
        {
            if (_movementController != null)
            {
                try
                {
                    _movementController.Rotate(DirectionToPlayer);
                } catch (Exception e)
                {

                }
            }

            _eventController?.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, new EntityEventContext() { owner = gameObject });
        }
    } 
}
