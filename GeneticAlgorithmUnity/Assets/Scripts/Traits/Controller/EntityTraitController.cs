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
/*                foreach (Trait.Definition def in _trait.Definitions)
                {
                    
                }*/
            }
        }
    } 
}
