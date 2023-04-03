using UnityEngine;
using Game.Events;

namespace Game.Weapons
{
    public abstract class Weapon<T> : MonoBehaviour, IEventListener
        where T: WeaponData
    {
        [SerializeField]
        protected T _settings;
        protected EntityEventController _eventController;
        protected bool _canUse;

        public virtual void Initialize(T settings)
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