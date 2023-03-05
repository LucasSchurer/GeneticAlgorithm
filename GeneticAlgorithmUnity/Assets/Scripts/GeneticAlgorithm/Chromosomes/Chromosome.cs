using System.Collections.Generic;
using System.Linq;
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

                if (random <= GeneticAlgorithmManager.Instance.MutationRate)
                {
                    gene.Mutate();
                }
            }
        }

        private void MutateAll()
        {
            float random = Random.Range(0f, 1f);

            if (random <= GeneticAlgorithmManager.Instance.MutationRate)
            {
                foreach (Gene gene in _genes)
                {
                    gene.Mutate();
                }
            }
        }

        public abstract Chromosome Copy();

        public static T Crossover<T>(T[] parents)
            where T: Chromosome
        {
            T offspring = (T)parents[0].Copy();

            if (parents.Length == 1)
            {
                return offspring;
            }

            List<int> validPoints = Enumerable.Range(1, offspring._genes.Length).ToList();

            int[] crossoverPoints = new int[parents.Length > offspring._genes.Length ? offspring._genes.Length : parents.Length];

            for (int i = 0; i < crossoverPoints.Length; i++)
            {
                int randomIndex = Random.Range(0, validPoints.Count);
                crossoverPoints[i] = validPoints.ElementAt(randomIndex);
                validPoints.RemoveAt(randomIndex);
            }

            crossoverPoints = crossoverPoints.OrderBy(p => p).ToArray();

            int lastCrossoverPoint = 0;

            for (int i = 0; i < crossoverPoints.Length; i++)
            {
                for (int j = lastCrossoverPoint; j < crossoverPoints[i]; j++)
                {
                    offspring._genes[j] = parents[i]._genes[j];
                }

                lastCrossoverPoint = crossoverPoints[i];
            }

            return (T)offspring.Copy();
        }
    } 
}
