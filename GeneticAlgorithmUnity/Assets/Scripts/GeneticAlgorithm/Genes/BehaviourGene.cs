using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourGene : Gene
{
    public BehaviourType type;

    public BehaviourGene(BehaviourType type)
    {
        this.type = type;
    }

    public override Gene Copy()
    {
        return new BehaviourGene(type);
    }

    public override void Mutate()
    {
        Randomize();
    }

    public override void Randomize()
    {
        type = (BehaviourType)Random.Range(0, (int)BehaviourType.Count);
    }
}
