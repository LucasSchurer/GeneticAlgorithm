using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private Bullet _bullet;

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
            case Projectile.Type.Bullet:
                Projectile bullet = Instantiate(_bullet, _projectileContainer);
                bullet.Initialize(owner, direction);
                return bullet;

            default:
                return null;
        }
    }
}
