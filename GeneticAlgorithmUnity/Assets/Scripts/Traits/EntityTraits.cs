using Game.Events;
using UnityEngine;

namespace Game.Traits
{
    [CreateAssetMenu(fileName = "EntityTraits", menuName = "Traits/Entity Traits")]
    public class EntityTraits : Traits<EntityEventType, EntityEventContext>
    {
    } 
}
