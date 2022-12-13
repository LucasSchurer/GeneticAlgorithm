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

        public int id;
        public int generation;
        public int[] parents;
        public List<int> children;
        public float fitness;
        public float[] fitnessPropertiesValues;

        private void Awake()
        {
            _statisticsController = GetComponent<StatisticsController>();
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
                this.parents = new int[parents.Length];

                for (int i = 0; i < this.parents.Length; i++)
                {
                    this.parents[i] = parents[i];
                }
            }

            children = new List<int>();
        }

        public void UpdateFitness(FitnessProperty[] properties, float[] populationFitnessPropertiesValues)
        {
            fitness = 0f;

            if (fitnessPropertiesValues == null || populationFitnessPropertiesValues == null)
            {
                return;
            }

            for (int i = 0; i < populationFitnessPropertiesValues.Length; i++)
            {
                if (populationFitnessPropertiesValues[i] == 0)
                {
                    if (properties[i].Inverse)
                    {
                        fitness += 1 * properties[i].Weight;
                    }

                    continue;
                }

                float propertyValue = (fitnessPropertiesValues[i] / populationFitnessPropertiesValues[i]);

                if (properties[i].Inverse)
                {
                    propertyValue = 1 - propertyValue;
                }

                propertyValue *= properties[i].Weight;

                fitness += propertyValue;
            }
        }

        public void UpdateFitnessPropertiesValues(FitnessProperty[] properties)
        {
            if (_statisticsController)
            {
                fitnessPropertiesValues = new float[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    fitnessPropertiesValues[i] = _statisticsController.GetStatistic(properties[i].StatisticsType);
                }
            } else
            {
                fitnessPropertiesValues = null;
            }
        }
    } 
}
