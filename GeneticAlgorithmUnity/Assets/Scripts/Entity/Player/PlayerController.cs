using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Controller2D _controller2D;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _controller2D = GetComponent<Controller2D>();
        
    }

    private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(input);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            UseWeapon();
        }

        RotateWeapon();
    }

    /// <summary>
    /// Returns the normalized mouse direction.
    /// </summary>
    private Vector2 GetMouseDirection()
    {
        return (GetMousePosition() - transform.position).normalized;
    }

    private Vector3 GetMousePosition()
    {
        Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cameraPosition.z = 0;
        return cameraPosition;
    }


    private void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _controller2D.Move(direction * _player.MovementSpeed * Time.deltaTime);
        }
    }

    private void UseWeapon()
    {
        _player.weapon.Use(GetMouseDirection());
    }

    private void RotateWeapon()
    {
        if (_player.weapon != null)
        {
            _player.weapon.transform.LookAt(GetMousePosition());
        }
    }
}
