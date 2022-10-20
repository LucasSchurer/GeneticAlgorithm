using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rb;
    private Vector3 _direction;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _direction.x = input.x;
        _direction.z = input.z;
        _direction.Normalize();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            UseWeapon();
        }
    }

    private void FixedUpdate()
    {
        if (_player.canMove)
        {
            Move();
            Rotate();
        }
    }

    /// <summary>
    /// Returns the normalized mouse direction.
    /// </summary>
    private Vector3 GetMouseDirection()
    {
        return (GetMousePosition() - transform.position).normalized;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return cameraPosition;
    }

    private void Move()
    {
        if (_direction != Vector3.zero)
        {
            _rb.AddForce(_direction * _player.MovementSpeed * 10f, ForceMode.Force);
            _direction = Vector3.zero;
        }
    }

    private void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Quaternion smoothRotation = Quaternion.LookRotation(hit.point - transform.position);

            smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * 6);

            transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
        }
    }

    private void UseWeapon()
    {
        _player.weapon.Fire();
    }
}
