using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _allyLayer;
        [SerializeField]
        private LayerMask _allyShieldLayer;
        [SerializeField]
        private LayerMask _enemyLayer;
        [SerializeField]
        private LayerMask _enemyShieldLayer;
        [SerializeField]
        private bool _isPlayer = false;

        public LayerMask AllyLayer => _allyLayer;
        public LayerMask EnemyLayer => _enemyLayer;
        public LayerMask AllyShieldLayer => _allyShieldLayer;
        public LayerMask EnemyShieldLayer => _enemyShieldLayer;
        public bool IsPlayer => _isPlayer;
    } 
}
