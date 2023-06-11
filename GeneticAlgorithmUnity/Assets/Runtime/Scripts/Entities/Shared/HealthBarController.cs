using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class HealthBarController : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private EntityEventController _eventController;
        [SerializeField]
        private Transform _healthBar;
        [SerializeField]
        private bool _lookAtPlayer = true;
        private Transform _player;

        private void Start()
        {
            _player = Managers.GameManager.Instance.Player;
        }

        private void Update()
        {
            if (_lookAtPlayer && _player != null)
            {
                Vector3 directionToPlayer = (_player.position - transform.position).normalized;
                Quaternion newRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
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

        private void ChangeHealthBar(ref EntityEventContext ctx)
        {
            if (ctx.HealthChange != null)
            {
                Vector3 scale = _healthBar.localScale;
                scale.x = ctx.HealthChange.CurrentHealth / ctx.HealthChange.MaxHealth;
                _healthBar.localScale = scale;
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnHealthChange, ChangeHealthBar, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnHealthChange, ChangeHealthBar, EventExecutionOrder.After);
            }
        }
    } 
}
