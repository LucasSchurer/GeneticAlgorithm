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
        [Tooltip("Will only be used to player weapons. Enemies will ignore this property")]
        [SerializeField]
        protected float _recoilStrength = 0f;

        [Header("Display Setings")]
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _description;

        public float Cooldown => _cooldown;
        public float Damage => _damage;
        public float RecoilStrength => _recoilStrength;
        public string Name => _name;
        public string Description => _description;
    }
}

