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
        private LayerMask _enemyLayer;
        [SerializeField]
        private bool _isPlayer = false;

        public LayerMask AllyLayer => _allyLayer;
        public LayerMask EnemyLayer => _enemyLayer;
        public bool IsPlayer => _isPlayer;
    } 
}
