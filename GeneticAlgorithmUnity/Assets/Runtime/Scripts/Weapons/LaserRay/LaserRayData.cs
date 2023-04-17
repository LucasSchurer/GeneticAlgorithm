using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "LaserRayData")]
    public class LaserRayData : HitscanRangedWeaponData
    {
        [SerializeField]
        private float _firePreparingTime;
        [SerializeField]
        private float _delayBetweenPreparingAndFire;

        public float FirePreparingTime => _firePreparingTime;
        public float DelayBetweenPreparingAndFire => _delayBetweenPreparingAndFire;
    } 
}
