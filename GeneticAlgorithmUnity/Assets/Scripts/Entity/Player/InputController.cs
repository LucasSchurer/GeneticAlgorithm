using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private EntityEventController _eventController;
    private MovementController _movementController;
    private Vector3 _direction;

    private void Awake()
    {
        _eventController = GetComponent<EntityEventController>();
        _movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _direction.x = input.x;
        _direction.z = input.z;
        _direction.Normalize();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _eventController.EventTrigger(CombatEventType.OnAttack, new CombatEventContext() { owner = GetComponent<Entity>(), healthModifier = -1 });
        }
    }

    private void FixedUpdate()
    {
        _movementController?.Move(_direction);

        Vector3 mousePosition;

        if (TryGetMousePosition(out mousePosition))
        {
            _movementController?.Rotate((mousePosition - transform.position).normalized);
        }
    }

    private bool TryGetMousePosition(out Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            return true;
        } else
        {
            mousePosition = Vector3.zero;
            return false;
        }
    }
}
