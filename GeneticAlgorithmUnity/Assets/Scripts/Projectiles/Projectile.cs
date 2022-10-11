using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [HideInInspector]
        public GameObject owner;
        [SerializeField]
        protected ScriptableObjects.Projectile _settings;

        protected virtual void Awake()
        {
            if (_settings.timeToDespawn > 0)
            {
                Destroy(gameObject, _settings.timeToDespawn);
            }
        }
    } 
}
