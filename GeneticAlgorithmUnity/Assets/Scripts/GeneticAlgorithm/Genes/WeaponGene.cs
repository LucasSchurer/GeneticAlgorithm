using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGene : Gene
{
    public Weapon.Type type;

    public WeaponGene(Weapon.Type type)
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
        type = (Weapon.Type)Random.Range(0, (int)Weapon.Type.Count);
    }
}
