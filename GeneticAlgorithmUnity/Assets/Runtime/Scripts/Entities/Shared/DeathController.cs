using UnityEngine;
using Game.Events;

namespace Game.Entities.Shared
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
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            } else
            {
                Destroy(gameObject);
            }
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
                _eventController.AddListener(EntityEventType.OnDeath, OnDeath, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            if (_eventController != null)
            {
                _eventController.RemoveListener(EntityEventType.OnDeath, OnDeath, EventExecutionOrder.After);
            }
        }
    } 
}
