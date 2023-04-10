using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "DestroyEntityAfterTime", menuName = "Traits/Effects/Entity/Destroy Entity After Time")]
    public class DestroyEntityAfterTime : Effect<EntityEventContext>
    {
        [Header("Settings")]
        [SerializeField]
        private float _time;

        public override void Trigger(ref EntityEventContext ctx, int currentStacks = 1)
        {
            ctx.EventController.StartCoroutine(DestroyCoroutine(ctx.EventController));
        }

        private IEnumerator DestroyCoroutine(EntityEventController eventController)
        {
            yield return new WaitForSeconds(_time);
            
            eventController.TriggerEvent(EntityEventType.OnDeath, new EntityEventContext());
        }
    }
}
