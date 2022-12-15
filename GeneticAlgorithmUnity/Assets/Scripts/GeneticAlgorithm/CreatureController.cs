using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Entities;

namespace Game.GA
{
    /// <summary>
    /// Base class to specify a genetic algorithm creature.
    /// </summary>
    public class CreatureController : MonoBehaviour
    {
        private StatisticsController _statisticsController;
        public BaseEnemyChromosome chromosome;
        public Entities.AI.BehaviourType behaviourType;

        public CreatureData data;

        private void Awake()
        {
            _statisticsController = GetComponent<StatisticsController>();
            data = new CreatureData();
        }

        public void Initialize(float mutationRate, bool shouldRandomizeChromosome = true, BaseEnemyChromosome chromosome = null, int[] parents = null)
        {
            if (chromosome == null)
            {
                this.chromosome = new BaseEnemyChromosome(mutationRate, true);
            }
            else
            {
                this.chromosome = (BaseEnemyChromosome)chromosome.Copy();
            }

            if (shouldRandomizeChromosome)
            {
                this.chromosome.RandomizeGenes();
            }

            this.chromosome.ApplyGenes(this);

            if (parents != null)
            {
                data.parents = new int[parents.Length];

                for (int i = 0; i < data.parents.Length; i++)
                {
                    data.parents[i] = parents[i];
                }
            }

            data.children = new List<int>();
        }

        public void UpdateFitness(FitnessProperty[] properties, float[] populationMaxPropertiesValues)
        {
            data.fitness = 0f;

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
                        data.fitness += 1 * properties[i].Weight;
                    }

                    continue;
                }

                float propertyValue = (data.fitnessPropertiesValues[i] / populationMaxPropertiesValues[i]);

                if (properties[i].Inverse)
                {
                    propertyValue = 1 - propertyValue;
                }

                propertyValue *= properties[i].Weight;

                data.fitness += propertyValue;
            }
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
    } 
}
