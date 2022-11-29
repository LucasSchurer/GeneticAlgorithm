using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Entities.AI;

namespace Game.GA
{
    public class BaseEnemyChromosome : Chromosome
    {
        public BaseEnemyChromosome(float mutationRate, bool shouldMutateIndividually = false, Gene[] genes = null) : base(mutationRate, shouldMutateIndividually, genes)
        {
        }

        public enum Genes
        {
            Behaviour,
            Color,
            Size
        }

        protected override void SetGenes()
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.Color] = new ColorGene(new Color(1, 1, 1, 1));
            _genes[(int)Genes.Behaviour] = new BehaviourGene(BehaviourType.Aggressive);
        }

        protected override void SetGenes(Gene[] genes)
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.Color] = genes[(int)Genes.Color].Copy();
            _genes[(int)Genes.Behaviour] = genes[(int)Genes.Behaviour].Copy();
        }

        public override Chromosome Copy()
        {
            BaseEnemyChromosome copy = new BaseEnemyChromosome(_mutationRate, _shouldMutateIndividually, _genes);

            return copy;
        }
    }
}
