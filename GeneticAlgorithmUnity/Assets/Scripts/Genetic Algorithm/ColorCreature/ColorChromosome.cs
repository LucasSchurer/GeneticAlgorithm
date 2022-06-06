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
        Color,
        Size
    }

    protected override void SetGenes()
    {
        _genes = new Gene[(int)GeneType.Size];
        _genes[(int)GeneType.Color] = new ColorGene(Color.white);
    }
}
