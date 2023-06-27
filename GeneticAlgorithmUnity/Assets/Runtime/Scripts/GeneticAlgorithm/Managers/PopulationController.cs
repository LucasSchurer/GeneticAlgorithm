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
    public class PopulationController : EntitySpawner, IEventListener
    {
        [Header("References")]
        [SerializeField]
        private CreatureControllerReference _creaturePrefab;

        private CreatureController[] _creatures;
        private List<CreatureController> _creaturesRequest;

        private GeneticAlgorithmController _gaController;

        private bool _startedListening = false;

        private void Awake()
        {
            WaveManager waveManager = WaveManager.Instance;
            GeneticAlgorithmController gaController = GetComponent<GeneticAlgorithmController>();

            if (waveManager != null && gaController != null)
            {
                _creatures = new CreatureController[waveManager.waveSettings.enemiesPerWave];
                _creaturesRequest = new List<CreatureController>();
                _gaController = gaController;
            }

            if (!_startedListening)
            {
                StartListening();
            }
        }

        private void GetNewPopulation()
        {
            GenerationController generationController = _gaController.GenerationController;

            generationController.CreateGeneration();

            CreatureData[] creaturesData = generationController.GetCurrentGenerationCreaturesData();

            for (int i = 0; i < creaturesData.Length; i++)
            {
                if (_creatures[i] != null)
                {
                    Destroy(_creatures[i].transform.parent.gameObject);
                }

                _creatures[i] = Instantiate(_creaturePrefab).CreatureController;
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
                gameManager.GetEventController().AddListener(GameEventType.OnWaveStart, GeneratePopulation, EventExecutionOrder.Before);
                gameManager.GetEventController().AddListener(GameEventType.OnWaveEnd, KillPopulation, EventExecutionOrder.Before);
            }

            _startedListening = true;
        }

        private void KillPopulation(ref GameEventContext ctx)
        {
            foreach (CreatureController creature in _creatures)
            {
                if (creature != null)
                {
                    creature.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnDeath, new EntityEventContext());
                }
            }
        }

        public void StopListening()
        {
            GameManager gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveStart, GeneratePopulation, EventExecutionOrder.Before);
                gameManager.GetEventController().RemoveListener(GameEventType.OnWaveEnd, KillPopulation, EventExecutionOrder.Before);
            }

            _startedListening = false;
        }

        private void OnEnable()
        {
            if (!_startedListening)
            {
                StartListening();
            }
        }

        private void OnDisable()
        {
            StopListening();
        }

        public override Transform[] GetEntities()
        {
            int amount = _creaturesRequest.Count;
            Transform[] creatures = new Transform[amount];

            for (int i = 0; i < amount; i++)
            {
                CreatureController creature = _creaturesRequest[i];
                creature.gameObject.SetActive(true);
                creature.Initialize();

                creatures[i] = creature.transform;
            }

            _creaturesRequest.Clear();

            return creatures;
        }
    } 
}
