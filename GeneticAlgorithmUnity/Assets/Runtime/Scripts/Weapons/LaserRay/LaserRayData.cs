using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/LaserRayData")]
    public class LaserRayData : HitscanWeaponData
    {
        [SerializeField]
        private float _firePreparingTime;
        [SerializeField]
        private float _delayBetweenPreparingAndFire;

        public float FirePreparingTime => _firePreparingTime;
        public float DelayBetweenPreparingAndFire => _delayBetweenPreparingAndFire;
    } 
}
