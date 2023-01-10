using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class StatusEffect
    {
        public StatusEffectType type;
        public bool isActive = false;
        public float strength = 0;

        public StatusEffect(StatusEffectType type, float strength)
        {
            this.type = type;
            this.strength = strength;
            isActive = true;
        }
    } 
}
