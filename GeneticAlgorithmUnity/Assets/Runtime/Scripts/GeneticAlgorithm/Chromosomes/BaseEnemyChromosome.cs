using UnityEngine;
using Game.AI;
using System.Runtime.Serialization;

namespace Game.GA
{
    [DataContract(Name = "BaseEnemyChromosome", Namespace = "")]
    public class BaseEnemyChromosome : Chromosome
    {
        public BaseEnemyChromosome(GeneticAlgorithmController gaController, Gene[] genes = null) : base(gaController, genes)
        {
        }

        public enum Genes
        {
            Behaviour,
            Traits,
            Weapon,
            Size
        }

        protected override void SetGenes()
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.Behaviour] = new BehaviourGene(_gaController, BehaviourType.Reckless);
            _genes[(int)Genes.Traits] = new TraitsGene(_gaController, 1);
            _genes[(int)Genes.Weapon] = new WeaponGene(_gaController, Weapons.WeaponType.None);
        }

        protected override void SetGenes(Gene[] genes)
        {
            _genes = new Gene[(int)Genes.Size];            
            _genes[(int)Genes.Behaviour] = genes[(int)Genes.Behaviour].Copy();
            _genes[(int)Genes.Traits] = genes[(int)Genes.Traits].Copy();
            _genes[(int)Genes.Weapon] = genes[(int)Genes.Weapon].Copy();
        }

        public override Chromosome Copy()
        {
            BaseEnemyChromosome copy = new BaseEnemyChromosome(_gaController, _genes);

            return copy;
        }
        
        public void AddRandomTrait()
        {
            ((TraitsGene)_genes[(int)Genes.Traits]).AddRandomTrait();
        }

        public void UpdateTraitsWeights(float fitness)
        {
            ((TraitsGene)_genes[(int)Genes.Traits]).UpdateTraitsWeights(fitness);
        }
    }
}
