using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using System.Linq;

namespace Game.Traits
{
    public class TraitManager : Singleton<TraitManager>, IEventListener
    {
        [SerializeField]
        private Traits<EntityEventType, EntityEventContext> _entityTraits;
        [SerializeField]
        private TraitWeights _traitWeights;
        private Dictionary<TraitIdentifier, Trait<EntityEventType, EntityEventContext>> _entityTraitsDictionary;
        private TraitIdentifier[] _traitIdentifiers;
        protected override void SingletonAwake()
        {
            _entityTraitsDictionary = _entityTraits.BuildTraitsDictionary();
            _traitIdentifiers = _entityTraitsDictionary.Keys.ToArray();
        }

        public Trait<EntityEventType, EntityEventContext> GetEntityTrait(TraitIdentifier identifier)
        {
            if (_entityTraitsDictionary != null && _entityTraitsDictionary.TryGetValue(identifier, out Trait<EntityEventType, EntityEventContext> trait))
            {
                return trait;
            } else
            {
                return null;
            }
        }

        public TraitIdentifier GetRandomTraitIdentifier()
        {
            return _traitIdentifiers[Random.Range(0, _traitIdentifiers.Length)];
        }

        public int GetTraitMaxStacks(TraitIdentifier identifier)
        {
            return GetEntityTrait(identifier).maxStacks;
        }

        public TraitIdentifier SelectTraitAmongTraits(int amount, bool useWeight)
        {
            List<TraitIdentifier> uniqueTraits = GetRandomUniqueTraits(amount);

            if (!useWeight)
            {
                return uniqueTraits[Random.Range(0, uniqueTraits.Count)];
            }

            TraitIdentifier selectedTrait = TraitIdentifier.None;
            float selectedTraitWeight = 0f;
            float weightSum = 0;

            string debugMessage = "";

            foreach (TraitIdentifier trait in uniqueTraits)
            {
                float weight = GetTraitWeight(trait);
                debugMessage += "Trait: " + trait.ToString() + " Weight: " + weight + "\n";

                weightSum += weight;

                if (selectedTraitWeight <= weight)
                {
                    selectedTrait = trait;
                    selectedTraitWeight = weight;
                }
            }

            float selectedSum = Random.Range(0f, weightSum);
            weightSum = 0;

            debugMessage += "\nSelected Sum: " + selectedSum;

            foreach (TraitIdentifier trait in uniqueTraits)
            {
                weightSum += GetTraitWeight(trait);

                if (selectedSum < weightSum)
                {
                    selectedTrait = trait;
                    break;
                }
            }

            debugMessage += "\nSelected Trait: " + selectedTrait;

            Debug.Log(debugMessage);

            return selectedTrait;
        }

        private List<TraitIdentifier> GetRandomUniqueTraits(int amount)
        {
            if (amount > _traitIdentifiers.Length)
            {
                return new List<TraitIdentifier>(_traitIdentifiers);
            }

            List<TraitIdentifier> allTraits = new List<TraitIdentifier>(_traitIdentifiers);
            List<TraitIdentifier> uniqueTraits = new List<TraitIdentifier>();

            for (int i = 0; i < amount; i++)
            {
                int selectedIndex = Random.Range(0, allTraits.Count);

                uniqueTraits.Add(allTraits[selectedIndex]);
                allTraits.RemoveAt(selectedIndex);
            }

            return uniqueTraits;
        }


        public float GetTraitWeight(TraitIdentifier trait)
        {
            return _traitWeights.GetTraitWeight(trait);
        }

        public void ChangeTraitWeight(TraitIdentifier trait, float newWeight)
        {
            _traitWeights.ChangeTraitWeight(trait, newWeight);
        }
        private void OnApplicationQuitEvent(ref GameEventContext ctx)
        {
            _traitWeights.SaveWeights();
        }

        public void StartListening()
        {
            Managers.GameManager.Instance.eventController.AddListener(GameEventType.OnApplicationQuit, OnApplicationQuitEvent, EventExecutionOrder.After);
        }

        public void StopListening()
        {
            Managers.GameManager.Instance.eventController.RemoveListener(GameEventType.OnApplicationQuit, OnApplicationQuitEvent, EventExecutionOrder.After);
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
