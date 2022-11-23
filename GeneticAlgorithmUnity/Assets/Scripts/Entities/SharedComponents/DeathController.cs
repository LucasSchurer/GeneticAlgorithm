using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System;

namespace Game.Entities
{
    public class DeathController : MonoBehaviour, IEventListener
    {
        private EntityEventController _eventController;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
        }

        protected virtual void OnDeath(ref EntityEventContext ctx)
        {
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }

        public void StartListening()
        {
            if (_eventController != null)
            {
                _eventController.AddListener(EntityEventType.OnDeath, OnDeath);
            }
        }

        private void OnTest(ref EntityEventContext ctx)
        {
            Debug.Log("Damage reduction");
            ctx.healthModifier++;
        }

        public void StopListening()
        {
            if (_eventController != null)
            {
                _eventController.RemoveListener(EntityEventType.OnDeath, OnDeath);
            }
        }
    } 
}
