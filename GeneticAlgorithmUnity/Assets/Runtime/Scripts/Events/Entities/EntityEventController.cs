using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the events of a specific entity
/// Including method register/unregister and 
/// event invokes
/// </summary>
namespace Game.Events
{
    [RequireComponent(typeof(Entity))]
    public class EntityEventController : EventController<EntityEventType, EntityEventContext>
    {
        private Entity _entity;

        public Entity Entity => _entity;

        protected override void Awake()
        {
            base.Awake();

            _entity = GetComponent<Entity>();
        }

        protected override void AddEventControllerToContext(ref EntityEventContext ctx)
        {
            ctx.EventController = this;
        }
    }
}
