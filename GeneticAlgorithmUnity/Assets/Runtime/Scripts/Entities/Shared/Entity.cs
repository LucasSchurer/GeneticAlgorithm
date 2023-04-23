using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _allyLayer;
        [SerializeField]
        private LayerMask _enemyLayer;

        public LayerMask AllyLayer => _allyLayer;
        public LayerMask EnemyLayer => _enemyLayer;
    } 
}
