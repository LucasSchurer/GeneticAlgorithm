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
        public BaseEnemyChromosome chromosome;
        public Entities.AI.BehaviourType behaviourType;

        public CreatureData data;

        private void Awake()
        {
            _statisticsController = GetComponent<StatisticsController>();
        }

        public void Initialize(CreatureData data)
        {
            this.data = data;
            data.chromosome.ApplyGenes(this);
        }

        public void UpdateFitness(FitnessProperty[] properties, float[] populationMaxPropertiesValues)
        {
            /*data.Fitness = 0f;

            if (data.fitnessPropertiesValues == null || populationMaxPropertiesValues == null)
            {
                return;
            }

            for (int i = 0; i < populationMaxPropertiesValues.Length; i++)
            {
                if (populationMaxPropertiesValues[i] == 0)
                {
                    if (properties[i].Inverse)
                    {
                        data._fitness += 1 * properties[i].Weight;
                    }

                    continue;
                }

                float propertyValue = (data.fitnessPropertiesValues[i] / populationMaxPropertiesValues[i]);

                if (properties[i].Inverse)
                {
                    propertyValue = 1 - propertyValue;
                }

                propertyValue *= properties[i].Weight;

                data._fitness += propertyValue;
            }*/
        }

        public void UpdateFitnessPropertiesValues(FitnessProperty[] properties)
        {
            if (_statisticsController)
            {
                data.fitnessPropertiesValues = new float[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    data.fitnessPropertiesValues[i] = _statisticsController.GetStatistic(properties[i].StatisticsType);
                }
            } else
            {
                data.fitnessPropertiesValues = null;
            }
        }

        private void UpdateCreatureDataOnWaveEnd(ref GameEventContext ctx)
        {
            if (_statisticsController)
            {
                data.Fitness.UpdateRawFitnessValue(_statisticsController);
            }

            Traits.EntityTraitController traitController = GetComponent<Traits.EntityTraitController>();

            if (traitController)
            {
                data.Traits = traitController.GetTraitsIdentifiers();
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
