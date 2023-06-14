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
                _eventController.AddListener(EntityEventType.OnDamageDealt, OnDamageDealt, EventExecutionOrder.After);
                _eventController.AddListener(EntityEventType.OnDamageTaken, OnDamageTaken, EventExecutionOrder.After);
                _eventController.AddListener(EntityEventType.OnHealingDealt, OnHealingDealt, EventExecutionOrder.After);
                _eventController.AddListener(EntityEventType.OnHealingTaken, OnHealingTaken, EventExecutionOrder.After);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnHitTaken, OnHitTaken, EventExecutionOrder.After);
                _eventController.RemoveListener(EntityEventType.OnHitDealt, OnHitDealt, EventExecutionOrder.After);
                _eventController.RemoveListener(EntityEventType.OnDamageDealt, OnDamageDealt, EventExecutionOrder.After);
                _eventController.RemoveListener(EntityEventType.OnDamageTaken, OnDamageTaken, EventExecutionOrder.After);
                _eventController.RemoveListener(EntityEventType.OnHealingDealt, OnHealingDealt, EventExecutionOrder.After);
                _eventController.RemoveListener(EntityEventType.OnHealingTaken, OnHealingTaken, EventExecutionOrder.After);
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
            if (ctx.Damage != null)
            {
                ModifyStatistic(StatisticsType.NegativeHitsTaken, 1f);
            }

            if (ctx.Healing != null)
            {
                ModifyStatistic(StatisticsType.PositiveHitsTaken, 1f);
            }
        }

        private void OnHitDealt(ref EntityEventContext ctx)
        {
            if (ctx.Damage != null)
            {
                ModifyStatistic(StatisticsType.NegativeHitsDealt, 1f);
            }

            if (ctx.Healing != null)
            {
                ModifyStatistic(StatisticsType.PositiveHitsDealt, 1f);
            }
        }

        private void OnDamageTaken(ref EntityEventContext ctx)
        {
            if (ctx.Damage != null)
            {
                ModifyStatistic(StatisticsType.DamageTaken, ctx.Damage.Damage);
            }
        }

        private void OnDamageDealt(ref EntityEventContext ctx)
        {
            if (ctx.Damage != null)
            {
                ModifyStatistic(StatisticsType.DamageDealt, ctx.Damage.Damage);
                
                if (ctx.Damage.FriendlyFire)
                {
                    ModifyStatistic(StatisticsType.FriendlyFire, ctx.Damage.Damage);
                }
            }
        }

        private void OnHealingDealt(ref EntityEventContext ctx)
        {
            if (ctx.Healing != null)
            {
                ModifyStatistic(StatisticsType.HealingDealt, ctx.Healing.Healing);
            }
        }

        private void OnHealingTaken(ref EntityEventContext ctx)
        {
            if (ctx.Healing != null)
            {
                ModifyStatistic(StatisticsType.HealingTaken, ctx.Healing.Healing);
            }
        }

        #endregion
    }
}
