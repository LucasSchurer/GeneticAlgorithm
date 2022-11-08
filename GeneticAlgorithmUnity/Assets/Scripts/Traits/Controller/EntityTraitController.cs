using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits
{
    public class EntityTraitController : TraitController<EntityEventType, EntityEventContext, EntityEventController>
    {
        protected override EntityEventContext GetContextForWhenAddedTraits()
        {
            return new EntityEventContext()
            {
                owner = gameObject,
                other = null,
                eventController = _eventController
            };
        }
    } 
}
