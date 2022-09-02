using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chromosome
{
    /// <summary>
    /// Property that controls if the mutatation rate will be used only one time to each gene
    /// or will be calculated for each gene to apply mutation.
    /// </summary>
    protected bool _shouldMutateIndividually = false;
    protected float _mutationRate;
    protected Gene[] _genes;

    public Gene GetGene(int i) => _genes.Length > i ? _genes[i] : null;

    public Chromosome(float mutationRate, bool shouldMutateIndividually = false, Gene[] genes = null)
    {
        if (genes != null)
        {
            SetGenes(genes);
        } else
        {
            SetGenes();
        }

        _shouldMutateIndividually = shouldMutateIndividually;
        _mutationRate = mutationRate;
    }

    protected abstract void SetGenes();
    protected abstract void SetGenes(Gene[] genes);

    public void RandomizeGenes()
    {
        foreach (Gene gene in _genes)
        {
            gene.Randomize();
        }
    }

    public void Mutate()
    {
        if (_shouldMutateIndividually)
        {
            MutateIndividually();
        } else
        {
            MutateAll();
        }
    }

    private void MutateIndividually()
    {
        foreach (Gene gene in _genes)
        {
            float random = Random.Range(0f, 1f);

            if (random <= _mutationRate)
            {
                gene.Mutate();
            }
        }
    }

    private void MutateAll()
    {
        float random = Random.Range(0, 1);

        if (random <= _mutationRate)
        {
            foreach (Gene gene in _genes)
            {
                gene.Mutate();
            }
        }
    }

    public abstract Chromosome Copy();

    public static System.Tuple<Chromosome, Chromosome> Crossover(Chromosome a, Chromosome b)
    {
        int crossoverPoint = Random.Range(0, a._genes.Length - 1);

        Chromosome offspringA = a.Copy();
        Chromosome offspringB = b.Copy();

        for (int i = 0; i < crossoverPoint; i++)
        {
            offspringA._genes[i] = b._genes[i];
            offspringB._genes[i] = a._genes[i];
        }

        System.Tuple<Chromosome, Chromosome> offspring = new System.Tuple<Chromosome, Chromosome>(offspringA, offspringB);

        return offspring;
    }
}
