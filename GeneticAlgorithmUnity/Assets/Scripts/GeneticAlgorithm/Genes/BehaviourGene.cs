using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities.AI;

namespace Game.GA
{
    public class BehaviourGene : Gene
    {
        public BehaviourType type;

        public BehaviourGene(BehaviourType type)
        {
            this.type = type;
        }

        public override void Apply(CreatureController creature)
        {
            creature.behaviourType = type;

            if (type == BehaviourType.Aggressive)
            {
                creature.GetComponent<Weapons.Rifle>().enabled = false;
            }
        }

        public override Gene Copy()
        {
            return new BehaviourGene(type);
        }

        public override void Mutate()
        {
            Randomize();
        }

        public override void Randomize()
        {
            type = (BehaviourType)Random.Range(0, (int)BehaviourType.Count);
        }
    } 
}
