using Game.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Game.GA
{
    [DataContract(Name = "BehaviourGene", Namespace = "")]
    [KnownType(typeof(WeaponGene))]
    public class WeaponGene : Gene
    {
        [DataMember(Name = "BehaviourType")]
        public WeaponType type;

        public WeaponGene(WeaponType type)
        {
            this.type = type;
        }

        public override void Apply(CreatureController creature)
        {
            WeaponManager.Instance.AddWeaponComponent(creature.gameObject, type, WeaponManager.WeaponHolder.Enemy);
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
            type = WeaponManager.Instance.GetRandomWeaponType(WeaponManager.WeaponHolder.Enemy);
        }
    }
}
