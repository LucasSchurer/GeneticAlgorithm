using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponDefinition
    {
        public WeaponOld weapon;
        public Game.ScriptableObjects.Weapon settings;
    }

    [SerializeField]
    private WeaponDefinition[] _weapons;

    [Header("Prefabs")]
    [SerializeField]
    private RifleOld _riflePrefab;
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
    public WeaponOld CreateWeapon(Entity owner, WeaponOld.Type type)
    {
        switch (type)
        {
            case WeaponOld.Type.Rifle:
                RifleOld rifle = Instantiate(_riflePrefab, owner.transform);
                rifle.Initialize(owner, null);
                owner.weapon = rifle;
                return rifle;

            case WeaponOld.Type.Sniper:
                Sniper sniper = Instantiate(_sniperPrefab, owner.transform);
                sniper.Initialize(owner, null);
                owner.weapon = sniper;
                return sniper;

            default:
                return null;
        }
    }
}
