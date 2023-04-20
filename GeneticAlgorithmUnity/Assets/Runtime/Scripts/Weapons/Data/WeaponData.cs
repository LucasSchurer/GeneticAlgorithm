using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/BaseData")]
    public class WeaponData : ScriptableObject
    {
        [Header("General Settings")]
        [Tooltip("Time between weapon uses")]
        [SerializeField]
        protected float _cooldown;
        [SerializeField]
        protected float _damage;

        public float Cooldown => _cooldown;
        public float Damage => _damage;
    }
}

