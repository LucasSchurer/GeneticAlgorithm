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
        private Traits<EntityEventType, EntityEventContext> _enemyTraits;
        [SerializeField]
        private Traits<EntityEventType, EntityEventContext> _playerTraits;

        public enum TraitHolder
        {
            Player,
            Enemy
        }

        public enum Team
        {   
            Team1, 
            Team2
        }

        [SerializeField]
        private TraitWeights _team1traitWeights;
        [SerializeField]
        private TraitWeights _team2traitWeights;

        private Dictionary<TraitIdentifier, Trait<EntityEventType, EntityEventContext>> _enemyTraitsDict;
        private Dictionary<TraitIdentifier, Trait<EntityEventType, EntityEventContext>> _playerTraitsDict;

        private TraitIdentifier[] _enemyTraitIdentifiers;
        private TraitIdentifier[] _playerTraitIdentifiers;

        protected override void SingletonAwake()
        {
            _enemyTraitsDict = _enemyTraits.BuildTraitsDictionary();
            _enemyTraitIdentifiers = _enemyTraitsDict.Keys.ToArray();

            _playerTraitsDict = _playerTraits.BuildTraitsDictionary();
            _playerTraitIdentifiers = _playerTraitsDict.Keys.ToArray();
        }

        public Trait<EntityEventType, EntityEventContext> GetEntityTrait(TraitIdentifier identifier, TraitHolder holder)
        {
            Dictionary<TraitIdentifier, Trait<EntityEventType, EntityEventContext>> dict = holder == TraitHolder.Player ? _playerTraitsDict : _enemyTraitsDict;

            if (dict != null && dict.TryGetValue(identifier, out Trait<EntityEventType, EntityEventContext> trait))
            {
                return trait;
            } else
            {
                if (dict.TryGetValue(TraitIdentifier.Default, out Trait<EntityEventType, EntityEventContext> defaultTrait))
                {
                    return defaultTrait;
                } else
                {
                    return null;
                }
            }
        }

        public TraitIdentifier GetRandomTraitIdentifier(TraitHolder holder)
        {
            TraitIdentifier[] identifiers = holder == TraitHolder.Player ? _playerTraitIdentifiers : _enemyTraitIdentifiers;

            return identifiers[Random.Range(0, identifiers.Length)];
        }

        public int GetTraitMaxStacks(TraitIdentifier identifier, TraitHolder holder)
        {
            return GetEntityTrait(identifier, holder).maxStacks;
        }

        public TraitIdentifier SelectTraitAmongTraits(int amount, bool useWeight, Team team)
        {
            List<TraitIdentifier> uniqueTraits = GetRandomUniqueTraits(amount, TraitHolder.Enemy);

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
                float weight = GetTraitWeight(trait, team);
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
                weightSum += GetTraitWeight(trait, team);

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

        public List<TraitIdentifier> GetRandomUniqueTraits(int amount, TraitHolder holder)
        {
            TraitIdentifier[] identifiers = holder == TraitHolder.Player ? _playerTraitIdentifiers : _enemyTraitIdentifiers;

            if (amount > identifiers.Length)
            {
                return new List<TraitIdentifier>(identifiers);
            }

            List<TraitIdentifier> allTraits = new List<TraitIdentifier>(identifiers);
            List<TraitIdentifier> uniqueTraits = new List<TraitIdentifier>();

            for (int i = 0; i < amount; i++)
            {
                int selectedIndex = Random.Range(0, allTraits.Count);

                uniqueTraits.Add(allTraits[selectedIndex]);
                allTraits.RemoveAt(selectedIndex);
            }

            return uniqueTraits;
        }

        public TraitWeights GetTraitWeights(Team team)
        {
            if (team == Team.Team1)
            {
                return _team1traitWeights;
            }
            else
            {
                return _team2traitWeights;
            }
        }

        public float GetTraitWeight(TraitIdentifier trait, Team team)
        {
            if (team == Team.Team1)
            {
                return _team1traitWeights.GetTraitWeight(trait);
            } else
            {
                return _team2traitWeights.GetTraitWeight(trait);
            }
        }

        public void ChangeTraitWeight(TraitIdentifier trait, float newWeight, Team team)
        {
            if (team == Team.Team1)
            {
                _team1traitWeights.ChangeTraitWeight(trait, newWeight);
            }
            else
            {
                _team2traitWeights.ChangeTraitWeight(trait, newWeight);
            }
        }
        private void OnApplicationQuitEvent(ref GameEventContext ctx)
        {
            _team1traitWeights.SaveWeights();
            _team2traitWeights.SaveWeights();
        }

        public void StartListening()
        {
            Managers.GameManager.Instance.GetEventController().AddListener(GameEventType.OnApplicationQuit, OnApplicationQuitEvent, EventExecutionOrder.After);
        }

        public void StopListening()
        {
            Managers.GameManager.Instance.GetEventController().RemoveListener(GameEventType.OnApplicationQuit, OnApplicationQuitEvent, EventExecutionOrder.After);
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
