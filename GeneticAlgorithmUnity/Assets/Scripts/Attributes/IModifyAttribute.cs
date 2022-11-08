using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IModifyAttribute
    {
        public abstract float GetCurrentValue();
        public abstract float GetMaximumValue();
        public abstract float GetMinimumValue();
        public abstract void ModifyMaximumValue(float changeAmount);
        public abstract void ModifyCurrentValue(float changeAmount);
        public abstract void ModifyMinimumValue(float changeAmount);
    } 
}
