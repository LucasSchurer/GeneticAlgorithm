using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGene : Gene
{
    public WeaponOld.Type type;

    public WeaponGene(WeaponOld.Type type)
    {
        this.type = type;
    }

    public override Gene Copy()
    {
        return new WeaponGene(type);
    }

    public override void Mutate()
    {
        Randomize();
    }

    public override void Randomize()
    {
        type = (WeaponOld.Type)Random.Range(0, (int)WeaponOld.Type.Count);
    }
}
