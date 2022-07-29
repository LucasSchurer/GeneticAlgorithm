using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public delegate void OnHit(Entity self, Entity other, float damage);
    public OnHit onHit;

    public delegate void WhenHit(Entity self, Entity other, float damage);
    public WhenHit whenHit;

    public delegate void OnKill(Entity self, Entity other);
    public OnKill onKill;

    public delegate void WhenKilled(Entity self, Entity other);
    public WhenKilled whenKilled;

    [SerializeField]
    protected float _health;
    [SerializeField]
    protected float _maxHealth;
    [SerializeField]
    protected float _movementSpeed;
    [SerializeField]
    protected Vector2 _position;
    [SerializeField]
    protected bool isDead = false;
    public bool canMove = true;

    public float MovementSpeed => _movementSpeed;

    public Weapon weapon;

    public LayerMask enemyLayerMask;
    public LayerMask selfLayerMask;
    public LayerMask obstacleLayerMask;

    public virtual void Damage(Entity source, float damage)
    {
        if (damage > 0)
        {
            _health -= damage;

            whenHit?.Invoke(this, source, damage);
            source.onHit?.Invoke(source, this, damage);

            if (_health <= 0)
            {
                whenKilled?.Invoke(this, source);
                source.onKill?.Invoke(source, this);
                Killed();
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
