using Game.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class Shield : MonoBehaviour, IEventListener
    {
        private Rigidbody _rb;
        private ShieldDeployerData _data;
        private EntityEventController _eventController;

        private float _currentDurability;

        public Rigidbody Rigidbody => _rb;

        public void Initialize(ShieldDeployerData data)
        {
            _data = data;

            _rb = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
        }
        private void OnHitTaken(ref EntityEventContext ctx)
        {
            
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
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnHitTaken, OnHitTaken);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnHitTaken, OnHitTaken);
            }
        }
    } 
}
