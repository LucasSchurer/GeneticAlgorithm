using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour, IEventListener
{
    [SerializeField]
    private Health _health;

    private EntityEventController _eventController;

    private float _maxHealth;
    [SerializeField]
    private float _currentHealth;

    public float MaxHealth { get => _maxHealth; private set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }

    private void Awake()
    {
        _eventController = GetComponent<EntityEventController>();
        CurrentHealth = _health.maxHealth;
    }

    private void OnHitTaken(CombatEventContext ctx)
    {
        CurrentHealth += ctx.healthModifier;

        if (CurrentHealth <= 0)
        {
            _eventController.EventTrigger(CombatEventType.OnDeath, ctx);
        }
    }

    private void OnEnable()
    {
        StartListening();
    }

    private void OnDisable()
    {
        StopListening();
    }

    public void StartListening()
    {
        if (_eventController != null)
        {
            _eventController.AddListener(CombatEventType.OnHitTaken, OnHitTaken);
        }
    }

    public void StopListening()
    {
        if (_eventController != null)
        {
            _eventController.RemoveListener(CombatEventType.OnHitTaken, OnHitTaken);
        }
    }
}
