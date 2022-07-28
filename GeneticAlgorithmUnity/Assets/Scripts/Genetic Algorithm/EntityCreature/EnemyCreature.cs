using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreature : Creature
{
    private EnemyChromosome _enemyChromosome;

    public override Chromosome Chromosome
    {
        get => _chromosome; 
        set
        {
            _chromosome = value;
            _enemyChromosome = (EnemyChromosome)_chromosome;
        }
    }

    public override void InitializeChromosomes()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeComponents()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateCreature()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateValues()
    {
        throw new System.NotImplementedException();
    }

    protected override float FitnessFunction()
    {
        throw new System.NotImplementedException();
    }
}
