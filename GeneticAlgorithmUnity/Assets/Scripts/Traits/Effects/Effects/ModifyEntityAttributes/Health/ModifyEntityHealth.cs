using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;
using Game.Entities;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "ModifyEntityHealth", menuName = "Traits/Effects/Entity/Modify Health")]
    public class ModifyEntityHealth : ModifyAttributeEffect<EntityEventContext, HealthController>
    {
    } 
}
