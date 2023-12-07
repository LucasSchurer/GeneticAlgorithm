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

        public BehaviourGene(GeneticAlgorithmController gaController, BehaviourType type)
        {
            _gaController = gaController;
            this.type = type;
        }

        public override void Apply(CreatureController creature)
        {
            StateMachine stateMachine = creature.GetComponent<StateMachine>();

            stateMachine.Initialize(BehaviourManager.Instance.GetBehaviourStateMachineData(this.type));
        }

        public override Gene Copy()
        {
            return new BehaviourGene(_gaController, type);
        }

        public override void Mutate()
        {
            Randomize();
        }

        public override void Randomize()
        {
            type = BehaviourManager.Instance.GetRandomBehaviourType();
        }

        public override void Randomize(System.Random rand)
        {
            type = BehaviourManager.Instance.GetRandomBehaviourType(rand);
        }
    } 
}
