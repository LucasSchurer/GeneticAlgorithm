using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class AttributeModifyEffect<Context, AttributeModifierComponent> : Effect<Context>
        where Context : EventContext
        where AttributeModifierComponent: IAttributeModifier
    {
        [SerializeField]
        protected float amount;

        public override void Trigger(ref Context ctx)
        {
            ctx.owner.GetComponent<AttributeModifierComponent>()?.ModifyCurrentValue(amount);
        }
    }
}
