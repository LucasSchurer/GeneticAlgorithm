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
            RedColor,
            GreenColor,
            BlueColor,
            Size
        }

        protected override void SetGenes()
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.RedColor] = new RGBValueGene(1f);
            _genes[(int)Genes.GreenColor] = new RGBValueGene(1f);
            _genes[(int)Genes.BlueColor] = new RGBValueGene(1f);
            _genes[(int)Genes.Behaviour] = new BehaviourGene(BehaviourType.Aggressive);
        }

        protected override void SetGenes(Gene[] genes)
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.RedColor] = genes[(int)Genes.RedColor].Copy();
            _genes[(int)Genes.GreenColor] = genes[(int)Genes.GreenColor].Copy();
            _genes[(int)Genes.BlueColor] = genes[(int)Genes.BlueColor].Copy();
            _genes[(int)Genes.Behaviour] = genes[(int)Genes.Behaviour].Copy();
        }

        public override Chromosome Copy()
        {
            BaseEnemyChromosome copy = new BaseEnemyChromosome(_mutationRate, _shouldMutateIndividually, _genes);

            return copy;
        }
    }
}
