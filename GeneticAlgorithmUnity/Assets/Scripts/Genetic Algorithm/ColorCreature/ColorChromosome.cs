using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChromosome : Chromosome
{
    public ColorChromosome(float mutationRate, bool shouldMutateIndividually = false, Gene[] genes = null) : base(mutationRate, shouldMutateIndividually, genes)
    {
    }

    public enum GeneType
    {
        RedColor,
        GreenColor,
        BlueColor,
        Position,
        Size
    }

    protected override void SetGenes()
    {
        _genes = new Gene[(int)GeneType.Size];
        _genes[(int)GeneType.RedColor] = new RGBValueGene(1f);
        _genes[(int)GeneType.GreenColor] = new RGBValueGene(1f);
        _genes[(int)GeneType.BlueColor] = new RGBValueGene(1f);
        _genes[(int)GeneType.Position] = new PositionGene(new Vector3(3, 3, 3), new Vector3(-20, -20, -20), new Vector3(20, 20, 20));
    }

    protected override void SetGenes(Gene[] genes)
    {
        _genes = new Gene[(int)GeneType.Size];
        _genes[(int)GeneType.RedColor] = genes[(int)GeneType.RedColor].Copy();
        _genes[(int)GeneType.GreenColor] = genes[(int)GeneType.GreenColor].Copy();
        _genes[(int)GeneType.BlueColor] = genes[(int)GeneType.BlueColor].Copy();
        _genes[(int)GeneType.Position] = genes[(int)GeneType.Position].Copy();
    }

    public override Chromosome Copy()
    {
        ColorChromosome copy = new ColorChromosome(_mutationRate, _shouldMutateIndividually, _genes);

        return copy;
    }

    public Color GetColor ()
    {
        return new Color
        {
            r = ((RGBValueGene)_genes[(int)GeneType.RedColor]).value,
            g = ((RGBValueGene)_genes[(int)GeneType.GreenColor]).value,
            b = ((RGBValueGene)_genes[(int)GeneType.BlueColor]).value,
            a = 1f
        };
    }

    public Vector3 GetPosition()
    {
        return ((PositionGene)_genes[(int)GeneType.Position]).position;
    }
}
