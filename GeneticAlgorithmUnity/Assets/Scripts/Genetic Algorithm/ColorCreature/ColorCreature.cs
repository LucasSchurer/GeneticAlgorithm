using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorCreature : Creature
{
    [SerializeField]
    private Color _desiredColor;

    private SpriteRenderer _spriteRenderer;
    private ColorChromosome _colorChromosome;

    [SerializeField]
    private TextMeshPro _fitnessText;

    public override void Spawn(bool randomizeChromosome)
    {
        base.Spawn(randomizeChromosome);

        transform.position = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), 0);
        _spriteRenderer.color = _colorChromosome.GetColor();
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

        fitness = Mathf.Pow(16, (3 - fitnessR - fitnessG - fitnessB)/3);

        _fitnessText.text = fitness.ToString();

        return fitness;
    }

    public override void InitializeChromosomes()
    {
        if (_chromosome == null)
        {
            _chromosome = new ColorChromosome(_mutationRate, true);
        }
            
        _colorChromosome = (ColorChromosome)_chromosome;
    }

    public override void InitializeComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
