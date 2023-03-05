using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Weapons;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "DisableEntityPrimaryActionEffect", menuName = "Traits/Effects/Entity/Disable Primary Action")]
    public class DisableEntityPrimaryAction : Effect<EntityEventContext>
    {
        public override void Trigger(ref EntityEventContext ctx, int currentStacks = 1)
        {
            Rifle rifle = ctx.owner.GetComponent<Rifle>();

            if (rifle)
            {
                rifle.enabled = false;
            }
        }
    } 
}
