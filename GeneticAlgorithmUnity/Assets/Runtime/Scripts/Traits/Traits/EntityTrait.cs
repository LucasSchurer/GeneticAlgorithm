using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits
{
    [CreateAssetMenu(fileName = "EntityTrait", menuName = "Traits/EntityTrait")]
    public class EntityTrait : Trait<EntityEventType, EntityEventContext>
    {

    } 
}
