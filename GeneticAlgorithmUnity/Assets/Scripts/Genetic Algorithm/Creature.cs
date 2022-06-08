using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    protected Chromosome _chromosome;
    [SerializeField]
    protected float _mutationRate;
    [SerializeField]
    public float _fitnessValue;

    public virtual Chromosome Chromosome { get => _chromosome; set => _chromosome = value; }

    protected abstract float FitnessFunction();

    public virtual float UpdateFitness()
    {
        _fitnessValue = FitnessFunction();

        return _fitnessValue;
    }

    /// <summary>
    /// Initialize the chromosome with the specific creature chromosome.
    /// </summary>
    public abstract void InitializeChromosomes();

    /// <summary>
    /// Initialize the creature's components. Will be called inside Spawn() method.
    /// </summary>
    public abstract void InitializeComponents();

    /// <summary>
    /// Creature behaviour who will be called every frame on Population Update().
    /// </summary>
    public abstract void UpdateCreature();

    /// <summary>
    /// Method called when a new generation starts. Treat as the object's Awake method.
    /// </summary>
    public virtual void Spawn(bool randomizeChromosome = false)
    {
        InitializeChromosomes();

        if (randomizeChromosome)
        {
            RandomizeChromosome();
        }

        InitializeComponents();
        UpdateValues();
    }

    public abstract void UpdateValues();

    private void RandomizeChromosome()
    {
        _chromosome.RandomizeGenes();
    }

    public void ClearFitnessValue()
    {
        _fitnessValue = 0;
    }
}
