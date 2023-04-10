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
                Owner = gameObject,
                Other = null,
                EventController = _eventController
            };
        }
    } 
}
