using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "InstantiateObject", menuName = "Traits/Effects/Entity/Instantiate Object")]
    public class InstantiateObject : VisualEffect<EntityEventContext>
    {
        [Header("Settings")]
        [SerializeField]
        private GameObject _object;
        [SerializeField]
        private TargetType _targetType;
        [SerializeField]
        private float _innerRadius;
        [SerializeField]
        private float _outerRadius;

        public override void Trigger(ref EntityEventContext ctx, int currentStacks = 1)
        {
            if (_object != null && EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                float angle = Random.Range(0, Mathf.PI * 2);
                float radius = Random.Range(_innerRadius, _outerRadius);

                Vector3 position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                position += target.transform.position;

                Instantiate(_object, position, Quaternion.identity);
            }
        }
    } 
}
