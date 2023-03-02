using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public abstract class Chromosome
    {
        /// <summary>
        /// Property that controls if the mutation rate will be used only one time to all genes
        /// or will be calculated for each gene individually
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
            }
            else
            {
                SetGenes();
            }

            _shouldMutateIndividually = shouldMutateIndividually;
            _mutationRate = mutationRate;
        }

        protected abstract void SetGenes();
        protected abstract void SetGenes(Gene[] genes);
        
        public virtual void ApplyGenes(CreatureController creature)
        {
            foreach (Gene gene in _genes)
            {
                gene.Apply(creature);
            }
        }

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
            }
            else
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
            float random = Random.Range(0f, 1f);

            if (random <= _mutationRate)
            {
                foreach (Gene gene in _genes)
                {
                    gene.Mutate();
                }
            }
        }

        public abstract Chromosome Copy();

        public static Chromosome[] Crossover(Chromosome a, Chromosome b)
        {
            int crossoverPoint = Random.Range(0, a._genes.Length - 1);

            Chromosome offspringA = a.Copy();
            Chromosome offspringB = b.Copy();

            for (int i = 0; i < crossoverPoint; i++)
            {
                offspringA._genes[i] = b._genes[i];
                offspringB._genes[i] = a._genes[i];
            }

            Chromosome[] offspring = new Chromosome[2] { offspringA, offspringB };

            return offspring;
        }
    } 
}
