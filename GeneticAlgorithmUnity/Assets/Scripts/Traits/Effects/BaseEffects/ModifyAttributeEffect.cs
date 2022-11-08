using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public abstract class ModifyAttributeEffect<Context, AttributeModifierComponent> : Effect<Context>
        where Context : EventContext
        where AttributeModifierComponent: IModifyAttribute
    {
        [Header("Settings")]
        [SerializeField]
        protected TargetType _targetType;
        [SerializeField]
        protected AttributeValueType _targetAttributeValue;
        [SerializeField]
        protected float _changeAmount;

        public override void Trigger(ref Context ctx)
        {
            if (EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                switch (_targetAttributeValue)
                {
                    case AttributeValueType.Current:
                        target.GetComponent<AttributeModifierComponent>()?.ModifyCurrentValue(_changeAmount);
                        break;
                    case AttributeValueType.Maximum:
                        target.GetComponent<AttributeModifierComponent>()?.ModifyMaximumValue(_changeAmount);
                        break;
                    case AttributeValueType.Minimum:
                        target.GetComponent<AttributeModifierComponent>()?.ModifyCurrentValue(_changeAmount);
                        break;
                }
            }
        }
    }
}
