using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    protected override void Killed()
    {
        
    }

    protected override void OnHitEvent(Entity entity, float damage, Projectile projectile = null)
    {
        
    }

    protected override void OnKillEvent(Entity entity, Projectile projectile = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnWeaponFiredEvent()
    {
        throw new System.NotImplementedException();
    }

    protected override void WhenHitEvent(Entity entity, float damage, Projectile projectile = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void WhenKilledEvent(Entity entity, Projectile projectile = null)
    {
        throw new System.NotImplementedException();
    }
}
