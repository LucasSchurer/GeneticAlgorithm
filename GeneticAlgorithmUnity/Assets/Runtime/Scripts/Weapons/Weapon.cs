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
        protected Entity _entity;
        protected bool _canUse = true;

        public void SetData(Data data)
        {
            _data = data;
        }

        protected virtual void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _socketController = GetComponent<EntitySocketController>();
            _entity = GetComponent<Entity>();
        }

        protected virtual void Start()
        {
            SetSocketsAndVFXs();
            SetLayers();
        }

        protected abstract void SetSocketsAndVFXs();
        protected abstract void SetLayers();

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