using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private Rifle _riflePrefab;
    [SerializeField]
    private Sniper _sniperPrefab;

    public static WeaponManager Instance { get; private set; }

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
    /// Instantiate and assign the created weapon to a given owner.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Weapon CreateWeapon(Entity owner, Weapon.Type type)
    {
        switch (type)
        {
            case Weapon.Type.Rifle:
                Rifle rifle = Instantiate(_riflePrefab, owner.transform);
                rifle.Initialize(owner);
                owner.weapon = rifle;
                return rifle;

            case Weapon.Type.Sniper:
                Sniper sniper = Instantiate(_sniperPrefab, owner.transform);
                sniper.Initialize(owner);
                owner.weapon = sniper;
                return sniper;

            default:
                return null;
        }
    }
}
