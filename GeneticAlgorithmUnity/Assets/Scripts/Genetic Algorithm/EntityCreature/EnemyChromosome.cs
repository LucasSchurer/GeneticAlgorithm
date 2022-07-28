using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChromosome : Chromosome
{
    public EnemyChromosome(float mutationRate, bool shouldMutateIndividually = false, Gene[] genes = null) : base(mutationRate, shouldMutateIndividually, genes)
    {
    }

    public enum Genes
    {
        Behaviour,
        Weapon,
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
        _genes[(int)Genes.Weapon] = new WeaponGene(Weapon.Type.Rifle);
    }

    protected override void SetGenes(Gene[] genes)
    {
        _genes = new Gene[(int)Genes.Size];
        _genes[(int)Genes.RedColor] = genes[(int)Genes.RedColor].Copy();
        _genes[(int)Genes.GreenColor] = genes[(int)Genes.GreenColor].Copy();
        _genes[(int)Genes.BlueColor] = genes[(int)Genes.BlueColor].Copy();
        _genes[(int)Genes.Behaviour] = genes[(int)Genes.Behaviour].Copy();
        _genes[(int)Genes.Weapon] = genes[(int)Genes.Weapon].Copy();
    }

    public Color GetColor()
    {
        return new Color
        {
            r = ((RGBValueGene)_genes[(int)Genes.RedColor]).value,
            g = ((RGBValueGene)_genes[(int)Genes.GreenColor]).value,
            b = ((RGBValueGene)_genes[(int)Genes.BlueColor]).value,
            a = 1f
        };
    }

    public BehaviourType GetBehaviour()
    {
        return ((BehaviourGene)_genes[(int)Genes.Behaviour]).type;
    }

    public Weapon.Type GetWeapon()
    {
        return ((WeaponGene)_genes[(int)Genes.Weapon]).type;
    }

    public override Chromosome Copy()
    {
        EnemyChromosome copy = new EnemyChromosome(_mutationRate, _shouldMutateIndividually, _genes);

        return copy;
    }
}
