using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Entities
{
    public class StatisticsController : MonoBehaviour, IEventListener
    {
        public enum Type
        {
            OnHitTaken,
            OnHitDealt
        }

        private EntityEventController _eventController;
        private Dictionary<Type, float> _statistics;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _statistics = new Dictionary<Type, float>();
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
                _eventController.AddListener(EntityEventType.OnHitTaken, OnHitTaken, EventExecutionOrder.After);
                _eventController.AddListener(EntityEventType.OnHitDealt, OnHitDealt, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnHitTaken, OnHitTaken, EventExecutionOrder.After);
                _eventController.RemoveListener(EntityEventType.OnHitDealt, OnHitDealt, EventExecutionOrder.After);
            }
        }

        public float GetStatistic(Type type)
        {
            if (_statistics.TryGetValue(type, out float value))
            {
                return value;
            } else
            {
                return 0f;
            }
        }

        private void ModifyStatistic(Type type, float amount)
        {
            if (_statistics.ContainsKey(type))
            {
                _statistics[type] += amount;
            } else
            {
                _statistics.Add(type, amount);
            }
        }

        #region LISTEN METHODS

        private void OnHitTaken(ref EntityEventContext ctx) { ModifyStatistic(Type.OnHitTaken, 1f); }
        private void OnHitDealt(ref EntityEventContext ctx) { ModifyStatistic(Type.OnHitDealt, 1f); }

        #endregion
    }
}
