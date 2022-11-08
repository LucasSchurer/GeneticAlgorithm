using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    [CreateAssetMenu(fileName = "EntitySpawnProjectilesAroundTarget", menuName = "Traits/Effects/Entity/Spawn Projectiles Around Target")]
    public class EntitySpawnProjectilesAroundTarget : SpawnProjectilesAroundTarget<EntityEventContext>
    {
    } 
}
