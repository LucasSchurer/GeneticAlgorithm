using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game
{
    public class EntityTraitController : TraitController<EntityEventType, EntityEventContext, EntityEventController>
    {
        protected override void Awake()
        {
            base.Awake();

            if (_trait)
            {
                foreach (Trait.Definition def in _trait.Definitions)
                {
                    TraitAction trait = new TestTraitAction(this, def.settings);
                    _eventController?.AddListener(def.eventType, trait.Action, def.eventOrder);
                    _actions.Add(trait);
                }
            }
        }
    } 
}
