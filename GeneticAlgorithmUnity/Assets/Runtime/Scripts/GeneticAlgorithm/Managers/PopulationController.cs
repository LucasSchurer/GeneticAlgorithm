using System.Collections.Generic;
using UnityEngine;
using Game.Managers;
using Game.Events;

namespace Game.GA
{
    /// <summary>
    /// Controls the creation of populations,
    /// using selection, crossover and mutation methods.
    /// </summary>
    public class PopulationController : MonoBehaviour, IEventListener
    {
        [Header("References")]
        [SerializeField]
        private CreatureController _creaturePrefab;

        private CreatureController[] _creatures;
        private List<CreatureController> _creaturesRequest;

        private GeneticAlgorithmManager _gaManager;

        private void Awake()
        {
            WaveManager waveManager = WaveManager.Instance;
            GeneticAlgorithmManager gaManager = GeneticAlgorithmManager.Instance;

            if (waveManager != null && gaManager != null)
            {
                _creatures = new CreatureController[waveManager.waveSettings.enemiesPerWave];
                _creaturesRequest = new List<CreatureController>();
                _gaManager = gaManager;
            }
        }

        private void GetNewPopulation()
        {
            GenerationController generationController = _gaManager.GenerationController;

            generationController.CreateGeneration();

            CreatureData[] creaturesData = generationController.GetCurrentGenerationCreaturesData();

            for (int i = 0; i < creaturesData.Length; i++)
            {
                if (_creatures[i] != null)
                {
                    Destroy(_creatures[i].gameObject);
                }

                _creatures[i] = Instantiate(_creaturePrefab);
                _creatures[i].SetData(creaturesData[i]);

                _creaturesRequest.Add(_creatures[i]);
            }
        }

        public CreatureController RequestCreature(Vector3 position)
        {
            if (_creaturesRequest.Count > 0)
            {
                int index = Random.Range(0, _creaturesRequest.Count);

                CreatureController creature = _creaturesRequest[index];
                creature.transform.position = position;
                creature.gameObject.SetActive(true);
                creature.Initialize();

                _creaturesRequest.RemoveAt(index);

                return creature;
            }

            return null;
        }

        private void GeneratePopulation(ref GameEventContext ctx)
        {
            GetNewPopulation();
        }

        public void StartListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.eventController.AddListener(GameEventType.OnWaveStart, GeneratePopulation, EventExecutionOrder.Before);
                gameManager.eventController.AddListener(GameEventType.OnWaveEnd, KillPopulation, EventExecutionOrder.Before);
            }
        }

        private void KillPopulation(ref GameEventContext ctx)
        {
            foreach (CreatureController creature in _creatures)
            {
                creature.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnDeath, new EntityEventContext());
            }
        }

        public void StopListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.eventController.RemoveListener(GameEventType.OnWaveStart, GeneratePopulation, EventExecutionOrder.Before);
                gameManager.eventController.RemoveListener(GameEventType.OnWaveEnd, KillPopulation, EventExecutionOrder.Before);
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
