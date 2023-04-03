using UnityEngine;
using Game.AI;
using System.Runtime.Serialization;

namespace Game.GA
{
    [DataContract(Name = "BaseEnemyChromosome", Namespace = "")]
    public class BaseEnemyChromosome : Chromosome
    {
        public BaseEnemyChromosome(Gene[] genes = null) : base(genes)
        {
        }

        public enum Genes
        {
            Behaviour,
            Color,
            Traits,
            Size
        }

        protected override void SetGenes()
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.Color] = new ColorGene(new Color(1, 1, 1, 1));
            _genes[(int)Genes.Behaviour] = new BehaviourGene(BehaviourType.Aggressive);
            _genes[(int)Genes.Traits] = new TraitsGene(1);
        }

        protected override void SetGenes(Gene[] genes)
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.Color] = genes[(int)Genes.Color].Copy();
            _genes[(int)Genes.Behaviour] = genes[(int)Genes.Behaviour].Copy();
            _genes[(int)Genes.Traits] = genes[(int)Genes.Traits].Copy();
        }

        public override Chromosome Copy()
        {
            BaseEnemyChromosome copy = new BaseEnemyChromosome(_genes);

            return copy;
        }
    }
}
