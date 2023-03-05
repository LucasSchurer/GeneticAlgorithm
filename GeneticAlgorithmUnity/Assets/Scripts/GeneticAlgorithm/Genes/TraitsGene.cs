using UnityEngine;
using Game.Traits;
using Game.Events;

namespace Game.GA
{
    public class TraitsGene : Gene
    {
        private TraitIdentifier[] _traits;

        public TraitsGene(int startingSize = 0, TraitIdentifier[] traits = null)
        {
            if (traits != null)
            {
                _traits = new TraitIdentifier[traits.Length];
                System.Array.Copy(traits, _traits, traits.Length);
            } else if (startingSize > 0)
            {
                _traits = new TraitIdentifier[startingSize];

                for (int i = 0; i < _traits.Length; i++)
                {
                    if (TraitManager.Instance)
                    {
                        _traits[i] = TraitManager.Instance.GetRandomTraitIdentifier();
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
                    foreach (TraitIdentifier identifier in _traits)
                    {
                        Trait<EntityEventType, EntityEventContext> trait = TraitManager.Instance.GetEntityTrait(identifier);

                        if (trait)
                        {
                            traitController.AddTrait(trait);
                        }
                    }
                }
            }
        }

        public override Gene Copy()
        {
            return new TraitsGene(0, _traits);
        }

        public override void Mutate()
        {
            
        }

        public override void Randomize()
        {
            
        }
    } 
}
