using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Entities.Shared
{
    public class StatisticsController : MonoBehaviour, IEventListener
    {
        private EntityEventController _eventController;
        private Dictionary<StatisticsType, float> _statistics;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _statistics = new Dictionary<StatisticsType, float>();
        }

        public Dictionary<StatisticsType, float> Copy()
        {
            Dictionary<StatisticsType, float> copy = new Dictionary<StatisticsType, float>();

            foreach (KeyValuePair<StatisticsType, float> item in _statistics)
            {
                copy.Add(item.Key, item.Value);
            }

            return copy;
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

        public float GetStatistic(StatisticsType type)
        {
            if (_statistics.TryGetValue(type, out float value))
            {
                return value;
            } else
            {
                return 0f;
            }
        }

        private void ModifyStatistic(StatisticsType type, float amount)
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

        private void OnHitTaken(ref EntityEventContext ctx)
        { 
            ModifyStatistic(StatisticsType.HitsTaken, 1f);
            ModifyStatistic(StatisticsType.DamageTaken, -ctx.healthModifier);
        }

        private void OnHitDealt(ref EntityEventContext ctx)
        { 
            ModifyStatistic(StatisticsType.HitsDealt, 1f);
            ModifyStatistic(StatisticsType.DamageDealt, -ctx.healthModifier);
        }

        #endregion
    }
}
