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

    public struct Statistics
    {
        public float timeAlive;
        public float damageTaken;
        public float damageDealt;
        public float projectilesFired;
        public float projectilesTaken;
        public float hitCount;
        public float friendlyFireHits;
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

    public Statistics statistics;

    public float MovementSpeed => _movementSpeed;

    public Weapon weapon;

    public LayerMask enemyLayerMask;
    public LayerMask selfLayerMask;
    public LayerMask obstacleLayerMask;

    protected abstract void OnHitEvent(Entity entity, float damage, Projectile projectile = null);
    protected abstract void WhenHitEvent(Entity entity, float damage, Projectile projectile = null);
    protected abstract void OnKillEvent(Entity entity, Projectile projectile = null);
    protected abstract void WhenKilledEvent(Entity entity, Projectile projectile = null);
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

    public virtual void Damage(Entity source, float damage)
    {
        if (isDead)
        {
            return;
        }

        if (damage > 0)
        {
            _health -= damage;

            if (_health <= 0)
            {
                whenKilled?.Invoke(source);
                source.onKill?.Invoke(source);
            }
        }
    }

    public virtual void Heal(Entity source, float healing)
    {
        if (healing > 0)
        {
            _health = Mathf.Clamp(_health + healing, 0, _maxHealth);
        }
    }

    protected virtual void Killed()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    public void Knockback(Vector3 direction, float strength)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddForce(-transform.forward * strength, ForceMode.Impulse);
    }
}
