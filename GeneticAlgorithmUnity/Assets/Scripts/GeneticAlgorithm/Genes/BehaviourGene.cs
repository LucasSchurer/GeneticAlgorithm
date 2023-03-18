using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities.AI;
using System.Runtime.Serialization;

namespace Game.GA
{
    [DataContract(Name = "BehaviourGene", Namespace = "")]
    [KnownType(typeof(BehaviourGene))]
    public class BehaviourGene : Gene
    {
        [DataMember(Name = "BehaviourType")]
        public BehaviourType type;

        public BehaviourGene(BehaviourType type)
        {
            this.type = type;
        }

        public override void Apply(CreatureController creature)
        {
            creature.GetComponent<EnemyStateMachine>().BehaviourType = this.type;
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
