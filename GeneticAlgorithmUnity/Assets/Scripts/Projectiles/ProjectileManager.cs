using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private BasicProjectile _basicProjectile;
    [SerializeField]
    private BasicProjectile _fastProjectile;

    [Header("References")]
    [SerializeField]
    private Transform _projectileContainer;

    public static ProjectileManager Instance { get; private set; }

    private List<Projectile> _projectiles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Spawns a projectile, assigning its owner and initializing it.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Projectile SpawnProjectile(Weapon owner, Projectile.Type type, Vector2 direction)
    {
        switch (type)
        {
            case Projectile.Type.Basic:
                Projectile basic = Instantiate(_basicProjectile, _projectileContainer);
                basic.Initialize(owner, direction);
                return basic;

            case Projectile.Type.Fast:
                Projectile fast = Instantiate(_fastProjectile, _projectileContainer);
                fast.Initialize(owner, direction);
                return fast;

            default:
                return null;
        }
    }
}
