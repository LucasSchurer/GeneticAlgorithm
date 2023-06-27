using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Events;
using Game.Entities.Shared;

namespace Game.GA
{
    public class GenerationController : MonoBehaviour, IEventListener
    {
        private int _currentGeneration = 0;
        private int _currentCreatureId = 1;
        private Dictionary<int, GenerationData> _generations;
        
        public int CurrentGeneration => _currentGeneration;
        public GenerationData[] Generations => _generations.Values.ToArray();
        public GenerationData CurrentGenerationData => _generations[_currentGeneration];

        private GeneticAlgorithmController _gaController;

        public GenerationController()
        {
            _generations = new Dictionary<int, GenerationData>();
        }

        private void Awake()
        {
            _gaController = GetComponent<GeneticAlgorithmController>();
        }

        public void CreateGeneration()
        {
            _currentGeneration++;

            GenerationData generationData = new GenerationData();
            generationData.Number = _currentGeneration;

            _generations.TryAdd(generationData.Number, generationData);

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
                generationData.GenerationFitness = 0;
                Dictionary<StatisticsType, float> maxRawValues = GetMaxRawValues(generationData);

                foreach (CreatureData creatureData in generationData.Creatures.Values)
                {
                    creatureData.Fitness.UpdateFitness(maxRawValues);
                    generationData.GenerationFitness += creatureData.Fitness.Value;
                }
            }
        }

        private void UpdateGenerationTraitWeights(int generation)
        {
            if (_generations.TryGetValue(generation, out GenerationData generationData))
            {
                foreach (CreatureData creatureData in generationData.Creatures.Values)
                {
                    creatureData.Chromosome.UpdateTraitsWeights(creatureData.Fitness.Value);
                }

                generationData.SetTraitWeights(Traits.TraitManager.Instance.GetTraitWeights(_gaController.Team));
            }
        }

        private Dictionary<StatisticsType, float> GetMaxRawValues(GenerationData generation)
        {
            Dictionary<StatisticsType, float> maxRawValues = new Dictionary<StatisticsType, float>();

            foreach (CreatureData creatureData in generation.Creatures.Values)
            {
                foreach (Fitness.PartialValue partialValue in creatureData.Fitness.PartialValues)
                {
                    if (maxRawValues.ContainsKey(partialValue.Type))
                    {
                        if (maxRawValues[partialValue.Type] <= partialValue.RawValue)
                        {
                            maxRawValues[partialValue.Type] = partialValue.RawValue;
                        }
                    }
                    else
                    {
                        maxRawValues.Add(partialValue.Type, partialValue.RawValue);
                    }
                }
            }

            return maxRawValues;
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
                    CreatureData[] parents = RouletteWheelSelection(parentsGeneration, _gaController.ParentsAmount);

                    CreatureData newCreature = CreateCreatureData(_currentGeneration, parents);

                    newCreature.Chromosome.Mutate();
                    
                    if (_currentGeneration > 1)
                    {
                        newCreature.Chromosome.AddRandomTrait();
                    }

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
            CreatureData data = new CreatureData(_gaController);
            data.Id = _currentCreatureId;
            data.Generation = generation;
            data.Parents = parents;

            if (parents != null)
            {
                foreach (CreatureData parent in parents)
                {
                    parent.Children.Add(data);
                }

                data.Chromosome = Chromosome.Crossover(parents.Select(c => c.Chromosome).ToArray());
            } else
            {
                data.Chromosome = new BaseEnemyChromosome(_gaController);
                data.Chromosome.RandomizeGenes();
            }

            _currentCreatureId++;

            return data;
        }

        public CreatureData[] GetCurrentGenerationCreaturesData()
        {
            return CurrentGenerationData.Creatures.Values.ToArray();
        }

        public void AddGenerationData(GenerationData data)
        {
            _generations.TryAdd(data.Number, data);
        }

        private void AddCreatureDataToGeneration(CreatureData data)
        {
            _generations.TryAdd(data.Generation, new GenerationData());

            _generations[data.Generation].AddCreatureData(data);
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
                    float randomFitness = UnityEngine.Random.Range(0, generationData.GenerationFitness);
                    float fitnessRange = 0f;

                    foreach (CreatureData creatureData in generationData.Creatures.Values)
                    {
                        fitnessRange += creatureData.Fitness.Value;

                        if (fitnessRange >= randomFitness)
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

        private void OnWaveEnd(ref GameEventContext ctx)
        {
            UpdateGenerationFitness(_currentGeneration);
            UpdateGenerationTraitWeights(_currentGeneration);
        }

        public void StartListening()
        {
            Managers.GameManager gameManager = Managers.GameManager.Instance;

            if (gameManager != null && gameManager.GetEventController() != null)
            {
                gameManager.GetEventController().AddListener(GameEventType.OnWaveEnd, OnWaveEnd, EventExecutionOrder.Before);
            }
        }

        public void StopListening()
        {
            Managers.GameManager gameManager = Managers.GameManager.Instance;

            if (gameManager != null && gameManager.GetEventController() != null)
            {
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveEnd, OnWaveEnd, EventExecutionOrder.Before);
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
