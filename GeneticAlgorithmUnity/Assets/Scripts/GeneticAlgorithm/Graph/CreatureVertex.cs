using Game.Entities;
using Game.Traits;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class CreatureVertex
    {
        public CreatureData data;

        public CreatureVertex(CreatureController creature)
        {
            BuildData(creature);
        }

        private void BuildData(CreatureController creature)
        {
            data = creature.data;
        }
    } 
}
