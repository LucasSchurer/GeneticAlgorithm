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
        private EntityEventController _eventController;

        public override BehaviourType GetBehaviourType() { return BehaviourType.Stationary; }

        protected override void Awake()
        {
            base.Awake();

            _movementController = GetComponent<MovementController>();
            _eventController = GetComponent<EntityEventController>();
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
