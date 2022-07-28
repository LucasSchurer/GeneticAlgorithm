using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{    
    private Enemy _enemy;
    private BoxController2D _controller2D;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _controller2D = GetComponent<BoxController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UseWeapon();
    }

    private void UseWeapon()
    {
        Vector2 direction = (GameManager.Instance.PlayerPosition - transform.position).normalized;
        _enemy.weapon.Use(direction);
    }

    private void Move()
    {
        switch (_enemy.Behaviour)
        {
            case BehaviourType.Aggressive:
                AggressiveMovement();
                break;

            case BehaviourType.Cautious:
                CautiousMovement();
                break;

            default:
                StationaryMovement();
                break;
        }
    }

    private void AggressiveMovement()
    {
        Vector2 direction = (GameManager.Instance.PlayerPosition - transform.position).normalized;

        _controller2D.Move(direction * _enemy.MovementSpeed * Time.deltaTime);
    }

    private void CautiousMovement()
    {
        Vector2 direction = (GameManager.Instance.PlayerPosition - transform.position).normalized;

        if (Vector2.Distance(transform.position, GameManager.Instance.PlayerPosition) > 5f)
        {
            _controller2D.Move(direction * _enemy.MovementSpeed * Time.deltaTime);
        }
    }

    private void StationaryMovement()
    {

    }
}
