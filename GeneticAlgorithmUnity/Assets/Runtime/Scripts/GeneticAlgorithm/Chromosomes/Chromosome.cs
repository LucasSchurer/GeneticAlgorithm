using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace Game.GA
{
    [DataContract(Name = "Chromosome", Namespace = "")]
    public abstract class Chromosome
    {
        protected GeneticAlgorithmController _gaController;

        [DataMember(Name = "Genes")]
        protected Gene[] _genes;

        public Gene GetGene(int i) => _genes.Length > i ? _genes[i] : null;

        public Chromosome(GeneticAlgorithmController gaController, Gene[] genes = null)
        {
            _gaController = gaController;

            if (genes != null)
            {
                SetGenes(genes);
            }
            else
            {
                SetGenes();
            }
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

        public void RandomizeGenes(System.Random rand)
        {
            foreach (Gene gene in _genes)
            {
                gene.Randomize(rand);
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
            if (_gaController.MutateIndividually)
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

                if (random <= _gaController.MutationRate)
                {
                    gene.Mutate();
                }
            }
        }

        private void MutateAll()
        {
            float random = Random.Range(0f, 1f);

            if (random <= _gaController.MutationRate)
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
