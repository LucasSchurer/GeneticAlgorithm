using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector3 _velocity;

    [SerializeField]
    private float _distanceBetweenEnemies = 10f;

    [SerializeField]
    private float _detectionRange = 10f;

    private Enemy _enemy;
    private BoxController2D _controller2D;

    public bool followPlayer = false;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 DirectionToPlayer => GameManager.Instance.PlayerPosition - transform.position;

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _controller2D = GetComponent<BoxController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotateWeapon();
        UseWeapon();
    }

    private void RotateWeapon()
    {
        if (_enemy.weapon != null)
        {
            _enemy.weapon.transform.LookAt(GameManager.Instance.PlayerPosition);
        }
    }

    private void UseWeapon()
    {
        if (_enemy.weapon != null && HasLineOfSight())
        {
            Vector2 direction = (GameManager.Instance.PlayerPosition - transform.position).normalized;
            _enemy.weapon.Use(direction);
        }
    }

    private void Move()
    {
        _velocity = Vector3.zero;

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

        if (_velocity != Vector3.zero)
        {
            _controller2D.Move(_velocity * Time.deltaTime * _enemy.MovementSpeed);
        }
    }

    private void AggressiveMovement()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _distanceBetweenEnemies, LayerMask.GetMask("Enemy"));
        Vector3 direction = Vector3.zero;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != this)
            {
                direction += (transform.position - hit.transform.position) * Vector3.Distance(transform.position, hit.transform.position);
            }
        }

        if (Vector3.Distance(GameManager.Instance.PlayerPosition, transform.position) > 2f)
        {
            Vector3 playerDirection = (GameManager.Instance.PlayerPosition - transform.position);

            direction += playerDirection;
        }

        _velocity += direction.normalized;
    }

    private void CautiousMovement()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _distanceBetweenEnemies, LayerMask.GetMask("Enemy"));
        Vector3 direction = Vector3.zero;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != this)
            {
                direction += (transform.position - hit.transform.position) * Vector3.Distance(transform.position, hit.transform.position);
            }
        }

        Vector3 playerDirection = (GameManager.Instance.PlayerPosition - transform.position);
        float distance = Vector2.Distance(transform.position, GameManager.Instance.PlayerPosition);

        if (distance > 12f)
        {
            direction += playerDirection;
        } else if (distance < 8f)
        {
            direction -= playerDirection;
        }

        _velocity += direction.normalized;
    }

    private void StationaryMovement()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _distanceBetweenEnemies, LayerMask.GetMask("Enemy"));
        Vector3 direction = Vector3.zero;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != this)
            {
                direction += (transform.position - hit.transform.position) * Vector3.Distance(transform.position, hit.transform.position);
            }
        }

        _velocity += direction.normalized;
    }

    private bool HasLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionToPlayer, _detectionRange, _enemy.selfLayerMask | _enemy.obstacleLayerMask);
        return !hit;
    }
}
