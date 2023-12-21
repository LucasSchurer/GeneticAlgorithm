using UnityEngine;
using Game.Events;
using Game.AI.States;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "CreateAreaEffect", menuName = "Traits/Effects/Entity/Create Area Effect")]
    public class CreateAreaEffect : Effect<EntityEventContext>
    {
        [Header("Settings")]
        [SerializeField]
        private TargetType _targetType;
        [SerializeField]
        private FindTargetData.TargetType _effectTarget;

        public enum EffectType { Damage, Healing }

        [SerializeField]
        private EffectType _effectType;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private float _intensity;
        [SerializeField]
        private bool _canHitSelf;

        [SerializeField]
        private ParticleSystem _particleEffect;

        public override void Trigger(ref EntityEventContext ctx, int currentStacks = 1)
        {
            if (EffectsHelper.TryGetTarget(_targetType, ctx, out GameObject target))
            {
                Entity owner = ctx.Owner.GetComponent<Entity>();

                if (owner)
                {
                    LayerMask targetLayerMask = GetTargetLayerMask(_effectTarget, owner);

                    Collider[] hits = Physics.OverlapSphere(target.transform.position, _radius, targetLayerMask);

                    EntityEventContext.DamagePacket damagePacket = null;
                    EntityEventContext.HealingPacket healingPacket = null;

                    switch (_effectType)
                    {
                        case EffectType.Damage:
                            damagePacket = new EntityEventContext.DamagePacket()
                            {
                                DamageType = Events.DamageType.Explosive,
                                Damage = _intensity,
                            };

                            break;
                        case EffectType.Healing:
                            healingPacket = new EntityEventContext.HealingPacket
                            {
                                Healing = _intensity,
                                HealingType = HealingType.Trait
                            };
                            break;
                    }

                    ParticleSystem particle = Instantiate(_particleEffect, target.transform.position, Quaternion.identity);
                    ParticleSystem.MainModule main = particle.main;

                    main.startSize = _radius * 2f;
                    particle.Play();

                    foreach (Collider hit in hits)
                    {
                        if ((hit.transform == owner.transform && _canHitSelf) || hit.transform != owner.transform)
                        {
                            if (owner && ctx.Other != null)
                            {
                                ctx.Other.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = ctx.Owner, Damage = damagePacket, Healing = healingPacket });

                                EntityEventContext ownerCtx = new EntityEventContext();
                                ownerCtx.Other = ctx.Other;

                                owner.GetComponent<EntityEventController>()?.TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = ctx.Other.gameObject, Damage = damagePacket, Healing = healingPacket });
                            }
                        }
                    }
                }
            }
        }

        private LayerMask GetTargetLayerMask(FindTargetData.TargetType target, Events.Entity entity)
        {
            switch (target)
            {
                case FindTargetData.TargetType.AllAllies:
                    return entity.AllyLayer | entity.AllyShieldLayer;
                case FindTargetData.TargetType.AllyCreature:
                    return entity.AllyLayer;
                case FindTargetData.TargetType.AllyShield:
                    return entity.AllyShieldLayer;
                case FindTargetData.TargetType.AllEnemies:
                    return entity.EnemyLayer | entity.EnemyShieldLayer;
                case FindTargetData.TargetType.EnemyCreature:
                    return entity.EnemyLayer;
                case FindTargetData.TargetType.EnemyShield:
                    return entity.EnemyShieldLayer;
                case FindTargetData.TargetType.AllCreature:
                    return entity.AllyLayer | entity.EnemyLayer;
                case FindTargetData.TargetType.AllShield:
                    return entity.AllyShieldLayer | entity.EnemyShieldLayer;
                default:
                    return entity.AllyLayer;
            }
        }
    }
}
