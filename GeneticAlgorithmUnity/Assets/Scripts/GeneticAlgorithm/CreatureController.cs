using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Entities;
using System;

namespace Game.GA
{
    /// <summary>
    /// Base class to specify a genetic algorithm creature.
    /// </summary>
    public class CreatureController : MonoBehaviour, IEventListener
    {
        private StatisticsController _statisticsController;
        private CreatureData _data;

        private void Awake()
        {
            _statisticsController = GetComponent<StatisticsController>();
        }

        public void Initialize(CreatureData data)
        {
            this._data = data;
            data.Chromosome.ApplyGenes(this);
        }

        private void UpdateCreatureDataOnWaveEnd(ref GameEventContext ctx)
        {
            if (_statisticsController)
            {
                _data.Fitness.UpdateRawFitnessValue(_statisticsController);
            }

            Traits.EntityTraitController traitController = GetComponent<Traits.EntityTraitController>();

            if (traitController)
            {
                _data.Traits = traitController.GetTraitsIdentifiers();
            }
        }

        public void StartListening()
        {
            Managers.GameManager gameManager = Managers.GameManager.Instance;
            
            if (gameManager != null && gameManager.eventController != null)
            {
                gameManager.eventController.AddListener(GameEventType.OnWaveEnd, UpdateCreatureDataOnWaveEnd, EventExecutionOrder.Before);
            }
        }

        public void StopListening()
        {
            Managers.GameManager gameManager = Managers.GameManager.Instance;

            if (gameManager != null && gameManager.eventController != null)
            {
                gameManager.eventController.RemoveListener(GameEventType.OnWaveEnd, UpdateCreatureDataOnWaveEnd, EventExecutionOrder.Before);
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
