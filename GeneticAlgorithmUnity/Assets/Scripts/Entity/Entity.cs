using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public delegate void OnHit(Entity entity, float damage, Projectile projectile = null);
    public OnHit onHit;

    public delegate void WhenHit(Entity entity, float damage, Projectile projectile = null);
    public WhenHit whenHit;

    public delegate void OnKill(Entity entity, Projectile projectile = null);
    public OnKill onKill;

    public delegate void WhenKilled(Entity entity, Projectile projectile = null);
    public WhenKilled whenKilled;

    public delegate void OnWeaponFired();
    public OnWeaponFired onWeaponFired;

    [System.Serializable]
    public struct EntityStatistics
    {
        public float timeAlive;
        public float damageTaken;
        public float damageDealt;
        public int projectilesFired;
        public int projectilesTaken;
        public int hitCount;
        public int friendlyFireHits;
        public int killCount;
        public int deathCount;
        public float Accuracy => hitCount / projectilesFired;
    }

    [SerializeField]
    protected float _health;
    [SerializeField]
    protected float _maxHealth;
    [SerializeField]
    protected float _movementSpeed;
    [SerializeField]
    protected Vector2 _position;
    [SerializeField]
    public bool isDead = false;
    public bool canMove = true;

    [SerializeField]
    protected EntityStatistics _statistics;

    public EntityStatistics Statistics => _statistics;

    public float MovementSpeed => _movementSpeed;

    public Weapon weapon;

    public LayerMask enemyLayerMask;
    public LayerMask selfLayerMask;
    public LayerMask obstacleLayerMask;

    protected abstract void OnHitEvent(Entity target, float damage, Projectile projectile = null);
    protected abstract void WhenHitEvent(Entity attacker, float damage, Projectile projectile = null);
    protected abstract void OnKillEvent(Entity target, Projectile projectile = null);
    protected abstract void WhenKilledEvent(Entity attacker, Projectile projectile = null);
    protected abstract void OnWeaponFiredEvent();
    protected virtual void RegisterToEvents()
    {
        onHit += OnHitEvent;
        whenHit += WhenHitEvent;
        onKill += OnKillEvent;
        whenKilled += WhenKilledEvent;
        onWeaponFired += OnWeaponFiredEvent;
    }
    protected virtual void UnregisterToEvents()
    {
        onHit -= OnHitEvent;
        whenHit -= WhenHitEvent;
        onKill -= OnKillEvent;
        whenKilled -= WhenKilledEvent;
        onWeaponFired -= OnWeaponFiredEvent;
    }

    protected virtual void OnEnable()
    {
        RegisterToEvents();
    }

    protected virtual void OnDisable()
    {
        UnregisterToEvents();
    }

    /// <summary>
    /// Method to damage an entity. 
    /// Should by called by the attacked entity.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="damage"></param>
    /// <param name="projectile"></param>
    protected virtual void ReceiveDamage(Entity attacker, float damage, Projectile projectile = null)
    {
        if (damage > 0)
        {
            _health -= damage;

            if (_health <= 0)
            {
                whenKilled?.Invoke(attacker, projectile);
                attacker.onKill?.Invoke(this, projectile);
            }
        }
    }

    protected virtual void Heal(Entity source, float healing)
    {
        if (healing > 0)
        {
            _health = Mathf.Clamp(_health + healing, 0, _maxHealth);
        }
    }

    public void Knockback(Vector3 direction, float strength)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForce(-transform.forward * strength, ForceMode.Impulse);
    }
}
