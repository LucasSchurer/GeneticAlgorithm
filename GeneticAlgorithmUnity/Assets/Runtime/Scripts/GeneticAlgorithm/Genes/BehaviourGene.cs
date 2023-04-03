using UnityEngine;
using Game.AI;
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
            StateMachine stateMachine = creature.GetComponent<StateMachine>();

            stateMachine.Initialize(BehaviourManager.Instance.GetBehaviourStateMachineData(this.type));
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
