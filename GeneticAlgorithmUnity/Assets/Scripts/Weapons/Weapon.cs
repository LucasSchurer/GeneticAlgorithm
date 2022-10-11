using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Weapons
{
    public abstract class Weapon : MonoBehaviour, IEventListener
    {
        [SerializeField]
        protected ScriptableObjects.Weapon _settings;
        protected EntityEventController _eventController;

        public virtual void Initialize(ScriptableObjects.Weapon settings)
        {
            _settings = settings;
        }

        protected virtual void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
        }

        protected virtual void OnEnable()
        {
            StartListening();
        }

        protected virtual void OnDisable()
        {
            StopListening();
        }

        public virtual void StartListening() { }

        public virtual void StopListening() { }
    }
}