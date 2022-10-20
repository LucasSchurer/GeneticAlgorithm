using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 _direction;

    [SerializeField]
    private float _distanceBetweenEnemies = 10f;

    [SerializeField]
    private float _detectionRange = 10f;

    private Enemy _enemy;
    private Rigidbody _rb;

    private Vector3 DirectionToPlayer => GameManager.Instance.PlayerPosition - transform.position;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enemy.isDead)
        {
            UseWeapon();
        }
    }

    private void FixedUpdate()
    {
        if (_enemy.canMove && !_enemy.isDead)
        {
            Move();
            Rotate();
        }
    }

    private void Rotate()
    {
        Quaternion smoothRotation = Quaternion.LookRotation(GameManager.Instance.PlayerPosition - transform.position);

        smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * 6);

        transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
    }

    private void UseWeapon()
    {
        if (_enemy.weapon != null && HasLineOfSight())
        {
            _enemy.weapon.Fire();
        }
    }

    private void Move()
    {
        _direction = Vector3.zero;

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

        if (_direction != Vector3.zero)
        {
            _rb.AddForce(_direction * _enemy.MovementSpeed * 10f, ForceMode.Force);
            _direction = Vector3.zero;
        }
    }

    private void AggressiveMovement()
    {
        Vector3 direction = GetDirectionBasedOnNearbyEnemies();

        if (Vector3.Distance(GameManager.Instance.PlayerPosition, transform.position) > 2f)
        {
            Vector3 playerDirection = (GameManager.Instance.PlayerPosition - transform.position);

            direction += playerDirection;
        }

        _direction += direction.normalized;
    }

    private void CautiousMovement()
    {
        Vector3 direction = GetDirectionBasedOnNearbyEnemies();

        Vector3 playerDirection = (GameManager.Instance.PlayerPosition - transform.position);
        float distance = Vector2.Distance(transform.position, GameManager.Instance.PlayerPosition);

        if (distance > 12f)
        {
            direction += playerDirection;
        } else if (distance < 8f)
        {
            direction -= playerDirection;
        }

        _direction += direction.normalized;
    }

    private void StationaryMovement()
    {
        Vector3 direction = GetDirectionBasedOnNearbyEnemies();

        _direction += direction.normalized;
    }

    /// <summary>
    /// Change the enemy direction aiming to keep a minimum space between
    /// them, avoiding unnecessary collisions. 
    /// Returns the non-normalized direction vector.
    /// </summary>
    private Vector3 GetDirectionBasedOnNearbyEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _distanceBetweenEnemies, LayerMask.GetMask("Enemy"));
        Vector3 direction = Vector3.zero;

        foreach (Collider hit in hits)
        {
            if (hit.gameObject != this)
            {
                direction += (transform.position - hit.transform.position) * Vector3.Distance(transform.position, hit.transform.position);
            }
        }

        return direction;
    }

    private bool HasLineOfSight()
    {
        return !Physics.Raycast(transform.position, DirectionToPlayer, _detectionRange, _enemy.selfLayerMask | _enemy.obstacleLayerMask);
    }
}
