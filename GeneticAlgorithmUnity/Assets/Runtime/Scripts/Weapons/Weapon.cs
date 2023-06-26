using UnityEngine;
using Game.Events;
using Game.Entities.Shared;
using Game.Entities;

namespace Game.Weapons
{
    public abstract class Weapon<Data> : MonoBehaviour, IEventListener, IComponent
        where Data: WeaponData
    {
        [SerializeField]
        protected Data _data;
        [SerializeField]
        protected int _currentAmmunition;
        protected EntityEventController _eventController;
        protected EntitySocketController _socketController;
        protected AttributeController _attributeController;
        protected NonPersistentAttribute _rateOfFireMultiplier;
        protected Entity _entity;
        protected bool _canUse = true;

        public void SetData(Data data)
        {
            _data = data;
            _currentAmmunition = _data.Ammunition;
        }

        protected virtual void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _socketController = GetComponent<EntitySocketController>();
            _attributeController = GetComponent<AttributeController>();
            _entity = GetComponent<Entity>();
        }

        protected virtual void Start()
        {
            SetSocketsAndVFXs();
            SetLayers();
            SetNonPersistentAttributes();
        }

        protected virtual void SetNonPersistentAttributes() 
        {
            if (_attributeController)
            {
                _rateOfFireMultiplier = _attributeController.GetNonPersistentAttribute(AttributeType.RateOfFireMultiplier);
            } else
            {
                _rateOfFireMultiplier = new NonPersistentAttribute();
            }
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

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}