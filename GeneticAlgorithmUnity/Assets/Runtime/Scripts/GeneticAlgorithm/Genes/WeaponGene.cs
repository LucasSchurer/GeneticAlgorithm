using Game.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Game.GA
{
    [DataContract(Name = "WeaponGene", Namespace = "")]
    [KnownType(typeof(WeaponGene))]
    public class WeaponGene : Gene
    {
        [DataMember(Name = "WeaponType")]
        public WeaponType type;

        public WeaponGene(GeneticAlgorithmController gaController, WeaponType type)
        {
            _gaController = gaController;
            this.type = type;
        }

        public override void Apply(CreatureController creature)
        {
            WeaponManager.Instance.AddWeaponComponent(creature.gameObject, type, _gaController.WeaponTeam);
        }

        public override Gene Copy()
        {
            return new WeaponGene(_gaController, type);
        }

        public override void Mutate()
        {
            Randomize();
        }

        public override void Randomize()
        {
            type = WeaponManager.Instance.GetRandomWeaponType(_gaController.WeaponTeam);
        }
    }
}
