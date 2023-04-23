using UnityEngine;
using Game.Events;
using Game.Entities.Shared;

namespace Game.Weapons
{
    public abstract class Weapon<Data> : MonoBehaviour, IEventListener
        where Data: WeaponData
    {
        [SerializeField]
        protected Data _data;
        protected EntityEventController _eventController;
        protected EntitySocketController _socketController;
        protected bool _canUse = true;

        protected virtual void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _socketController = GetComponent<EntitySocketController>();
        }
        protected virtual void Start()
        {
            SetSocketsAndVFXs();
        }

        protected abstract void SetSocketsAndVFXs();

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