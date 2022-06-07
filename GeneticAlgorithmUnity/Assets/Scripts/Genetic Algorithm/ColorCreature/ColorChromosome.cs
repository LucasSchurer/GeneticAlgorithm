using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChromosome : Chromosome
{
    public ColorChromosome(float mutationRate, bool shouldMutateIndividually = false) : base(mutationRate, shouldMutateIndividually)
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
