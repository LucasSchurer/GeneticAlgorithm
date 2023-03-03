using Game.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.GA
{
    public class GenerationController : MonoBehaviour, IEventListener
    {
        private int _currentGeneration = 0;
        private int _currentCreatureId = 1;
        
        private Dictionary<int, GenerationData> _generations;
        public GenerationData[] Generations => _generations.Values.ToArray();
        public GenerationData CurrentGenerationData => _generations[_generations.Count];
        public GenerationController()
        {
            _generations = new Dictionary<int, GenerationData>();
        }
        public int CurrentGeneration => _currentGeneration;

        public void CreateGeneration()
        {
            _currentGeneration++;

            GenerationData generationData = new GenerationData();
            generationData.number = _currentGeneration;

            _generations.TryAdd(generationData.number, generationData);

            if (_currentGeneration == 1)
            {
                CreateFreshGeneration();
            } else
            {
                CreateGeneration(_currentGeneration - 1);
            }
        }
        
        /// <summary>
        /// Update partial values of each creature, resulting in it's final fitness value.
        /// Update the passed generation fitness.
        /// </summary>
        /// <param name="generation"></param>
        private void UpdateGenerationFitness(int generation)
        {
            if (_generations.TryGetValue(generation, out GenerationData generationData))
            {
                generationData.generationFitness = 0;
                Dictionary<Entities.StatisticsType, float> maxRawValues = new Dictionary<Entities.StatisticsType, float>();

                foreach (CreatureData creatureData in generationData.creatures.Values)
                {
                    foreach (Fitness.PartialValue partialValue in creatureData.Fitness.PartialValues)
                    {
                        if (maxRawValues.ContainsKey(partialValue.Type))
                        {
                            if (maxRawValues[partialValue.Type] <= partialValue.RawValue)
                            {
                                maxRawValues[partialValue.Type] = partialValue.RawValue;
                            }
                        } else
                        {
                            maxRawValues.Add(partialValue.Type, partialValue.RawValue);
                        }
                    }
                }

                foreach (CreatureData creatureData in generationData.creatures.Values)
                {
                    creatureData.Fitness.UpdateFitness(maxRawValues);
                    generationData.generationFitness += creatureData.Fitness.Value;
                }
            }
        }

        /// <summary>
        /// Method that generates a new set of 
        /// creatures, using selection, crossover
        /// and mutation methods.
        /// The newly generated creatures will be part of the
        /// last (and current) generation.
        /// </summary>
        private void CreateGeneration(int parentsGeneration)
        {
            GenerationData currentGeneration = CurrentGenerationData;

            if (_generations.ContainsKey(parentsGeneration) && currentGeneration != null)
            {
                int numberOfCreatures = Managers.WaveManager.Instance.waveSettings.enemiesPerWave;

                for (int i = 0; i < numberOfCreatures; i++)
                {
                    CreatureData[] parents = RouletteWheelSelection(parentsGeneration, 1);

                    CreatureData newCreature = CreateCreatureData(_currentGeneration, parents);

                    newCreature.chromosome.Mutate();

                    AddCreatureDataToGeneration(newCreature);
                }

            } else
            {
                Debug.LogWarning("Parent Generation Number is invalid. Won't create a new generation!");
            }
        }

        /// <summary>
        /// Creates a generation without any data from
        /// parents. 
        /// </summary>
        private void CreateFreshGeneration()
        {
            for (int i = 0; i < Managers.WaveManager.Instance.waveSettings.enemiesPerWave; i++)
            {
                AddCreatureDataToGeneration(CreateCreatureData(1));
            }
        }

        private CreatureData CreateCreatureData(int generation, CreatureData[] parents = null)
        {
            CreatureData data = new CreatureData();
            data.id = _currentCreatureId;
            data.generation = generation;
            data.isDead = false;
            data.Parents = parents;

            if (parents != null)
            {
                foreach (CreatureData parent in parents)
                {
                    parent.Children.Add(data);
                }

                data.chromosome = (BaseEnemyChromosome)parents[0].chromosome.Copy();
            } else
            {
                data.chromosome = new BaseEnemyChromosome(0.15f, false);
                data.chromosome.RandomizeGenes();
            }

            _currentCreatureId++;

            return data;
        }

        public CreatureData[] GetCurrentGenerationCreaturesData()
        {
            return CurrentGenerationData.creatures.Values.ToArray();
        }

        public void AddGenerationData(GenerationData data)
        {
            _generations.TryAdd(data.number, data);
        }

        private void AddCreatureDataToGeneration(CreatureData data)
        {
            _generations.TryAdd(data.generation, new GenerationData());

            _generations[data.generation].AddCreatureData(data);
        }

        public GenerationData GetGenerationData(int generation)
        {            
            if (generation < _generations.Count)
            {
                return _generations[generation];
            }
            else
            {
                throw new System.Exception("Generation out of bounds");
            }
        }

        /// <summary>
        /// Return n <see cref="CreatureData"/>
        /// selected by the roulette wheel selection method.
        /// <see href="https://en.wikipedia.org/wiki/Fitness_proportionate_selection"/>
        /// </summary>
        /// <returns></returns>
        private CreatureData[] RouletteWheelSelection(int generation, int amount = 1)
        {
            if (_generations.TryGetValue(generation, out GenerationData generationData))
            {
                List<CreatureData> parents = new List<CreatureData>();

                for (int i = 0; i < amount; i++)
                {
                    float randomFitness = UnityEngine.Random.Range(0, generationData.generationFitness);
                    float fitnessRange = 0f;

                    foreach (CreatureData creatureData in generationData.creatures.Values)
                    {
                        fitnessRange += creatureData.Fitness.Value;

                        if (fitnessRange > randomFitness)
                        {
                            parents.Add(creatureData);
                            break;
                        }
                    }
                }

                return parents.ToArray();

            } else
            {
                Debug.LogWarning("Generation Number is invalid. Won't select parents!");
                return null;
            }
        }

        private void UpdateCurrentGenerationFitnessOnWaveEnd(ref GameEventContext ctx)
        {
            UpdateGenerationFitness(_currentGeneration);
        }

        public void StartListening()
        {
            Managers.GameManager gameManager = Managers.GameManager.Instance;

            if (gameManager != null && gameManager.eventController != null)
            {
                gameManager.eventController.AddListener(GameEventType.OnWaveEnd, UpdateCurrentGenerationFitnessOnWaveEnd, EventExecutionOrder.Standard);
            }
        }

        public void StopListening()
        {
            Managers.GameManager gameManager = Managers.GameManager.Instance;

            if (gameManager != null && gameManager.eventController != null)
            {
                gameManager.eventController.RemoveListener(GameEventType.OnWaveEnd, UpdateCurrentGenerationFitnessOnWaveEnd, EventExecutionOrder.Standard);
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
