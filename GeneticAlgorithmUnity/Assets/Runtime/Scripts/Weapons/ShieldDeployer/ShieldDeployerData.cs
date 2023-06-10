using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/ShieldDeployerData")]
    public class ShieldDeployerData : WeaponData
    {
        [SerializeField]
        [Tooltip("If checked will reduce durability at each hit blocked, breaking if reaches 0")]
        private bool _canBreak;
        [SerializeField]
        private float _durability;

        [SerializeField]
        [Tooltip("If checked will despawn after n seconds")]
        private bool _canExpire;
        [SerializeField]
        private float _expirationTime;

        public bool CanBreak => _canBreak;
        public float Durability => _durability;
        public bool CanExpire => _canExpire;
        public float ExpirationTime => _expirationTime;
        
    }
}

