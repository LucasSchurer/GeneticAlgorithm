using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HomingOrbOnHit")]
    public class HomingOrbOnHit : ScriptableObject
    {
        [Tooltip("Value to be used as damage or health")]
        [SerializeField]
        private float _healthInteractionValue = 0f;

        public enum InteractionType
        {
            Damage,
            Heal
        }

        [SerializeField]
        private InteractionType _type;

        [SerializeField]
        private bool _canHitAllies;
        [SerializeField]
        private float _friendlyFireDamage;

        public void OnOrbHit(GameObject owner, GameObject other, Collider hit, GameObject orb)
        {
            EntityEventController otherController = other.GetComponent<EntityEventController>();

            if (otherController != null)
            {
                EntityEventContext.DamagePacket damagePacket = null;
                EntityEventContext.HealingPacket healingPacket = null;

                switch (_type)
                {
                    case InteractionType.Damage:
                        damagePacket = new EntityEventContext.DamagePacket()
                        {
                            DamageType = Events.DamageType.Default,
                            Damage = _healthInteractionValue,
                            ImpactPoint = hit.transform.position,
                            HitDirection = (hit.transform.position - orb.transform.position).normalized
                        };

                        if (_canHitAllies && (other.gameObject.layer == owner.gameObject.layer))
                        {
                            damagePacket.Damage *= _friendlyFireDamage;
                        }

                        break;
                    case InteractionType.Heal:
                        healingPacket = new EntityEventContext.HealingPacket
                        {
                            Healing = _healthInteractionValue,
                            HealingType = HealingType.Weapon
                        };
                        break;
                }

                otherController.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = owner, Damage = damagePacket, Healing = healingPacket });

                if (owner)
                {
                    EntityEventContext ownerCtx = new EntityEventContext();
                    ownerCtx.Other = other;

                    owner.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, Damage = damagePacket, Healing = healingPacket });
                }
            }
        }
    }
}
