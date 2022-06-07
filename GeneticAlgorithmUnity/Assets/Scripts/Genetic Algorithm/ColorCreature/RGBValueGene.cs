using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBValueGene : Gene
{
    public float value;

    public RGBValueGene(float value)
    {
        this.value = value;
    }

    public override void Mutate()
    {
        value = Random.Range(0f, 1f);
    }

    public override void Randomize()
    {
        value = Random.Range(0f, 1f);
    }
}
