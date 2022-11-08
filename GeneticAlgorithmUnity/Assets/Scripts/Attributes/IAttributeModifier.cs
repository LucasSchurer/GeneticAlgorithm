using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IAttributeModifier
    {
        public abstract float CurrentValue();
        public abstract float MaximumValue();
        public abstract void ModifyMaximumValue(float change);
        public abstract void ModifyCurrentValue(float change);
    } 
}
