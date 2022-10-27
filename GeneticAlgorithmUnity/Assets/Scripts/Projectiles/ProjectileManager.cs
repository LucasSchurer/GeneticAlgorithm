/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private BulletOld _bullet;

    [Header("References")]
    [SerializeField]
    private Transform _projectileContainer;

    public static ProjectileManager Instance { get; private set; }

    private List<ProjectileOld> _projectiles;

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
    public ProjectileOld SpawnProjectile(WeaponOld owner, Transform barrel, ProjectileOld.Type type, Quaternion? customRotation = null)
    {
        switch (type)
        {
            case ProjectileOld.Type.Bullet:
                ProjectileOld bullet = Instantiate(_bullet, barrel.position, customRotation ?? barrel.rotation, _projectileContainer);
                bullet.Initialize(owner);
                return bullet;

            default:
                return null;
        }
    }
}
*/