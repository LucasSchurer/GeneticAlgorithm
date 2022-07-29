using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    protected override void OnHitEvent(Entity target, float damage, Projectile projectile = null)
    {
        _statistics.damageDealt += damage;
    }

    protected override void OnKillEvent(Entity target, Projectile projectile = null)
    {
        _statistics.killCount++;
    }

    protected override void OnWeaponFiredEvent()
    {
        _statistics.projectilesFired++;
    }

    protected override void WhenHitEvent(Entity attacker, float damage, Projectile projectile = null)
    {
        /*ReceiveDamage(attacker, damage, projectile);*/
        _statistics.damageTaken += damage;
    }

    protected override void WhenKilledEvent(Entity attacker, Projectile projectile = null)
    {
        _statistics.deathCount++;
    }
}
