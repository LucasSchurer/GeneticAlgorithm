using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "EntitySpawnProjectilesTowardsTarget", menuName = "Traits/Effects/Entity/Spawn Projectiles Towards Target")]
    public class EntitySpawnProjectilesTowardsTarget : SpawnProjectilesTowardsTarget<EntityEventContext>
    {
    } 
}
