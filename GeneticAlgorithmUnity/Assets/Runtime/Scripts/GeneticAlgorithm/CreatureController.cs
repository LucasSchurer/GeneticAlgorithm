using UnityEngine;
using Game.Events;
using Game.Entities.Shared;

namespace Game.GA
{
    /// <summary>
    /// Base class to specify a genetic algorithm creature.
    /// </summary>
    public class CreatureController : MonoBehaviour, IEventListener
    {
        private StatisticsController _statisticsController;
        private EntityEventController _eventController;
        private CreatureData _data;

        private bool _isDead = false;

        public bool IsDead { get => _isDead; set => _isDead = value; }

        private void Awake()
        {
            _statisticsController = GetComponent<StatisticsController>();
            _eventController = GetComponent<EntityEventController>();
        }

        public void SetData(CreatureData data)
        {
            _data = data;
        }

        public void Initialize()
        {
            _data.Chromosome.ApplyGenes(this);
        }

        private void UpdateCreatureDataOnDeath(ref EntityEventContext ctx)
        {
            if (_statisticsController)
            {
                _data.Fitness.UpdateRawFitnessValue(_statisticsController);

                if (ctx.Damage != null && !_isDead)
                {
                    _isDead = true;
                }

                _data.IsDead = _isDead;
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnDeath, UpdateCreatureDataOnDeath, EventExecutionOrder.Before);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnDeath, UpdateCreatureDataOnDeath, EventExecutionOrder.Before);
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
    } 
}
