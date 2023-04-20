using UnityEngine;
using Game.Events;

namespace Game.Weapons
{
    public abstract class Weapon<Data> : MonoBehaviour, IEventListener
        where Data: WeaponData
    {
        [SerializeField]
        protected Data _data;
        protected EntityEventController _eventController;
        protected bool _canUse = true;

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