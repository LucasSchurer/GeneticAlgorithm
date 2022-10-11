using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour, IEventListener
{
    private EntityEventController _eventController;

    private void Awake()
    {
        _eventController = GetComponent<EntityEventController>();
    }

    private void OnDeath(EntityEventContext ctx)
    {
        Destroy(gameObject);
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
            _eventController.AddListener(EntityEventType.OnDeath, OnDeath);
        }
    }

    public void StopListening()
    {
        if (_eventController != null)
        {
            _eventController.RemoveListener(EntityEventType.OnDeath, OnDeath);
        }
    }
}
