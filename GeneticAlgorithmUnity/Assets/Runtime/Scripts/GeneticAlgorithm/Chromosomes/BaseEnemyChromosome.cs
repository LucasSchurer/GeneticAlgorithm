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
            Weapon,
            Size
        }

        protected override void SetGenes()
        {
            _genes = new Gene[(int)Genes.Size];
            _genes[(int)Genes.Color] = new ColorGene(new Color(1, 1, 1, 1));
            _genes[(int)Genes.Behaviour] = new BehaviourGene(BehaviourType.Reckless);
            _genes[(int)Genes.Traits] = new TraitsGene(1);
            _genes[(int)Genes.Weapon] = new WeaponGene(Weapons.WeaponType.None);
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
