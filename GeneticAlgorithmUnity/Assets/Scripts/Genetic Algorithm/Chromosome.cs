using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chromosome
{
    protected Gene[] _genes;

    public Gene GetGene(int i) => _genes.Length > i ? _genes[i] : null;

    public Chromosome()
    {
        SetGenes();
    }

    protected abstract void SetGenes();

    public void RandomizeGenes()
    {
        foreach (Gene gene in _genes)
        {
            gene.Randomize();
        }
    }

    public void Mutate()
    {

    }

    public static System.Tuple<Chromosome, Chromosome> Crossover(Chromosome a, Chromosome b)
    {
        int crossoverPoint = Random.Range(0, a._genes.Length-1);

        Chromosome offspringA = System.ObjectExtensions.Copy(a);
        Chromosome offspringB = System.ObjectExtensions.Copy(b);

        for (int i = 0; i < crossoverPoint; i++)
        {
            offspringA._genes[i] = b._genes[i];
            offspringB._genes[i] = a._genes[i];
        }

        System.Tuple<Chromosome, Chromosome> offspring = new System.Tuple<Chromosome, Chromosome>(offspringA, offspringB);

        return offspring;
    }
}
