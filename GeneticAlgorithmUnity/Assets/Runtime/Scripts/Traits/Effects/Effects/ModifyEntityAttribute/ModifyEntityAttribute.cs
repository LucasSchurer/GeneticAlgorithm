using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Entities;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "ModifyEntityAttributeEffect", menuName = "Traits/Effects/Entity/Modify Entity Attribute")]
    public class ModifyEntityAttribute : Effect<EntityEventContext>
    {
        private enum ValueType
        { 
            Maximum,
            Current,
            Minimum
        }

        [Header("Settings")]
        [SerializeField]
        private TargetType _targetType;
        [SerializeField]
        private AttributeType _attributeType;
        [SerializeField]
        private ValueType _valueType;
        [SerializeField]
        private float _changeAmount;

        public override void Trigger(ref EntityEventContext ctx, int currentStacks = 1)
        {
            if (EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                AttributeController attributeController = target.GetComponent<AttributeController>();

                if (attributeController)
                {
                    NonPersistentAttribute npAttribute = attributeController.GetNonPersistentAttribute(_attributeType);

                    if (npAttribute.Type != AttributeType.None)
                    {
                        switch (_valueType)
                        {
                            case ValueType.Current:
                                npAttribute.CurrentValue += _changeAmount;
                                break;
                            case ValueType.Maximum:
                                npAttribute.MaxValue += _changeAmount;
                                break;
                            case ValueType.Minimum:
                                npAttribute.MinValue += _changeAmount;
                                break;
                        }
                    }
                }
            }
        }

        public override void Trigger(GameObject owner, int currentStacks = 1)
        {
            Debug.Log("Teste");
        }
    }
}
