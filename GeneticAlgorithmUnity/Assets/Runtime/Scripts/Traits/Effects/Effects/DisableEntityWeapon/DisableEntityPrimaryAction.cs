using Game.Events;
using UnityEngine;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "DisableEntityPrimaryActionEffect", menuName = "Traits/Effects/Entity/Disable Primary Action")]
    public class DisableEntityPrimaryAction : Effect<EntityEventContext>
    {
        public override void Trigger(ref EntityEventContext ctx, int currentStacks = 1)
        {
            Weapons.Rifle rifle = ctx.owner.GetComponent<Weapons.Rifle>();

            if (rifle)
            {
                rifle.enabled = false;
            }
        }
    } 
}
