using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGene : Gene
{
    private Color _color;

    public Color Color => _color;

    public ColorGene(Color color)
    {
        _color = color;
    }

    public override void Mutate()
    {
        int mutatedValue = Random.Range(1, 3);

        switch (mutatedValue)
        {
            case 1:
                _color.r = Random.Range(0f, 1f);
                break;
            case 2:
                _color.g = Random.Range(0f, 1f);
                break;
            case 3:
                _color.b = Random.Range(0f, 1f);
                break;
            default:
                break;
        }
    }

    public override void Randomize()
    {
        _color.r = Random.Range(0f, 1f);
        _color.g = Random.Range(0f, 1f);
        _color.b = Random.Range(0f, 1f);
    }
}
