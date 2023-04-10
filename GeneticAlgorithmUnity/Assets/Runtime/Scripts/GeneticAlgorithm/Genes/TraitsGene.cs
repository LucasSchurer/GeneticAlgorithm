using UnityEngine;
using Game.Traits;
using Game.Events;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Game.GA
{
    [DataContract(Name = "TraitsGene", Namespace = "")]
    [KnownType(typeof(TraitsGene))]
    public class TraitsGene : Gene
    {
        private Dictionary<TraitIdentifier, int> _traits;

        [DataMember(Name = "Traits")]
        private List<SerializableDictionary<TraitIdentifier, int>> SerializableDictionary => SerializableDictionary<TraitIdentifier, int>.BuildListFromDictionary(_traits);

        [DataMember(Name = "ChosenTrait")]
        private TraitIdentifier _chosenTrait = TraitIdentifier.None;

        [DataMember(Name = "MutationRemovedGene")]
        private TraitIdentifier _removedTrait = TraitIdentifier.None;

        [DataMember(Name = "MutationAddedGene")]
        private TraitIdentifier _addedTrait = TraitIdentifier.None;

        public TraitsGene(int startingSize = 0, Dictionary<TraitIdentifier, int> traits = null)
        {
            if (traits != null)
            {
                _traits = new Dictionary<TraitIdentifier, int>(traits);
            } else if (startingSize > 0)
            {
                _traits = new Dictionary<TraitIdentifier, int>();

                for (int i = 0; i < startingSize; i++)
                {
                    if (TraitManager.Instance)
                    {
                        AddTrait(TraitManager.Instance.GetRandomTraitIdentifier());
                    }
                }
            }
        }

        public override void Apply(CreatureController creature)
        {
            if (_traits != null)
            {
                EntityTraitController traitController = creature.GetComponent<EntityTraitController>();

                if (traitController)
                {
                    foreach (KeyValuePair<TraitIdentifier, int> trait in _traits)
                    {
                        AddTraitToController(trait.Key, trait.Value, traitController);
                    }
                }
            }
        }

        public override Gene Copy()
        {
            return new TraitsGene(0, _traits);
        }

        /// <summary>
        /// Exchange one trait for another one.
        /// </summary>
        public override void Mutate()
        {
            TraitIdentifier removedTrait = _traits.Keys.ToArray()[Random.Range(0, _traits.Count - 1)];

            RemoveTrait(removedTrait);

            TraitIdentifier addedTrait = TraitManager.Instance.GetRandomTraitIdentifier();

            AddTrait(addedTrait);

            _removedTrait = removedTrait;
            _addedTrait = addedTrait;
        }

        public override void Randomize()
        {
            
        }

        private void AddTraitToController(TraitIdentifier identifier, int amount, EntityTraitController controller)
        {
            Trait<EntityEventType, EntityEventContext> trait = TraitManager.Instance.GetEntityTrait(identifier);

            if (trait)
            {
                for (int i = 0; i < amount; i++)
                {
                    controller.AddTrait(trait);
                }
            }
        }

        private void AddTrait(TraitIdentifier trait, int amount = 1)
        {
            int maxStacks = TraitManager.Instance.GetTraitMaxStacks(trait);

            if (_traits.ContainsKey(trait))
            {
                _traits[trait] = Mathf.Clamp(_traits[trait] + amount, 1, maxStacks);
            } else
            {
                _traits.Add(trait, Mathf.Clamp(amount, 1, maxStacks));
            }
        }

        private void RemoveTrait(TraitIdentifier trait, int amount = 1)
        {
            if (_traits.ContainsKey(trait))
            {
                _traits[trait] = _traits[trait] - amount;

                if (_traits[trait] <= 0)
                {
                    _traits.Remove(trait);
                }
            }
        }

        public void AddRandomTrait()
        {
            bool useWeights = true;

            if (Random.Range(0f, 1f) < GeneticAlgorithmManager.Instance.TraitSelectionDumbness)
            {
                useWeights = false;
            }

            TraitIdentifier chosenTrait = TraitManager.Instance.SelectTraitAmongTraits(GeneticAlgorithmManager.Instance.TraitSelectionAmount, useWeights);
            _chosenTrait = chosenTrait;

            AddTrait(chosenTrait);
        }

        public void UpdateTraitsWeights(float fitness)
        {
            float weightSign = 0;

            if (fitness < GeneticAlgorithmManager.Instance.NegativeTraitWeightChangeThreshold)
            {
                weightSign = -1;
            } else if (fitness > GeneticAlgorithmManager.Instance.PositiveTraitWeightChangeThreshold)
            {
                weightSign = 1;
            } else
            {
                return;
            }

            foreach (KeyValuePair<TraitIdentifier, int> trait in _traits)
            {
                float newWeight = TraitManager.Instance.GetTraitWeight(trait.Key);
                newWeight += (fitness * GeneticAlgorithmManager.Instance.TraitWeightChange) * weightSign;

                for (int i = 0; i < trait.Value; i++)
                {
                    TraitManager.Instance.ChangeTraitWeight(trait.Key, newWeight);
                }
            }
        }
    } 
}
