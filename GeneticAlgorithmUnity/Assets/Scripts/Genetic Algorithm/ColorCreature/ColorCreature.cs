using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorCreature : Creature
{
    [SerializeField]
    public Color _desiredColor;

    [SerializeField]
    private Transform _desiredPosition;

    private SpriteRenderer _spriteRenderer;

    private MeshRenderer _meshRenderer;

    private ColorChromosome _colorChromosome;

    [SerializeField]
    private TextMeshPro _fitnessText;

    public override Chromosome Chromosome { get =>_chromosome; set {
            _chromosome = value;
            _colorChromosome = (ColorChromosome)_chromosome;
        } }

    public override void UpdateValues()
    {
        /*transform.position = _colorChromosome.GetPosition();*/
        _spriteRenderer.color = _colorChromosome.GetColor();
    }

    public override void Spawn(bool randomizeChromosome = false)
    {
        base.Spawn(randomizeChromosome);

        transform.position = _colorChromosome.GetPosition();
    }

    public override void UpdateCreature()
    {
        
    }

    protected override float FitnessFunction()
    {
        float fitness = 0;

        Color currentColor = _colorChromosome.GetColor();

        float fitnessR = Mathf.Abs(currentColor.r - _desiredColor.r);
        float fitnessG = Mathf.Abs(currentColor.g - _desiredColor.g);
        float fitnessB = Mathf.Abs(currentColor.b - _desiredColor.b);

        float distanceFitness = 1 - Vector3.Distance(_colorChromosome.GetPosition(), _desiredPosition.position) / 100;
        float colorFitness = (3 - fitnessR - fitnessG - fitnessB) / 3;

        fitness = Mathf.Pow(4, distanceFitness + colorFitness);

        _fitnessText.text = fitness.ToString();

        return fitness;
    }

    public override void InitializeChromosomes()
    {
        if (Chromosome == null)
        {
            Chromosome = new ColorChromosome(_mutationRate, true);
        }
    }

    public override void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        /*_meshRenderer = GetComponent<MeshRenderer>();*/
    }
}
