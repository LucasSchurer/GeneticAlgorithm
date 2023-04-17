using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class CreatureControllerReference : MonoBehaviour
    {
        [SerializeField]
        private CreatureController _creatureController;

        public CreatureController CreatureController => _creatureController;
    } 
}
