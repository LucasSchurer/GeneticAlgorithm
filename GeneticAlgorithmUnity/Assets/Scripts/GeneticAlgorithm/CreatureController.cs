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

        public float fitness;
        public float[] fitnessPropertiesValues;

        private void Awake()
        {
            _statisticsController = GetComponent<StatisticsController>();
        }

        public void Initialize(float mutationRate, bool shouldRandomizeChromosome = true, BaseEnemyChromosome chromosome = null)
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

            UpdateChromosomeValues();
        }

        public void UpdateChromosomeValues()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            Color color = chromosome.GetColor();

            if (meshRenderer)
            {
                meshRenderer.material.color = color;
                meshRenderer.material.SetColor("_EmissionColor", color * 1.7f);
            }

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                MeshRenderer rbMeshRenderer = rb.GetComponent<MeshRenderer>();

                if (rbMeshRenderer)
                {
                    rbMeshRenderer.material.color = color;
                    rbMeshRenderer.material.SetColor("_EmissionColor", color * 1.7f);
                }
            }

            behaviourType = chromosome.GetBehaviour();

            if (behaviourType == Entities.AI.BehaviourType.Aggressive)
            {
                GetComponent<Weapons.Rifle>().enabled = false;
            }
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
                    continue;
                }

                fitness += (fitnessPropertiesValues[i] / populationFitnessPropertiesValues[i]) * properties[i].Weight;
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
