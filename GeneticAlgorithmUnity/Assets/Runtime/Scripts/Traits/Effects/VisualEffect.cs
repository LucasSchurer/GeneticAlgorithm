using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public class VisualEffect<Context> : Effect<Context>
        where Context : EventContext
    {
        [Header("Visual")]
        [SerializeField]
        private GameObject _visualEffect;

        public override void WhenAdded(GameObject owner, int currentStacks)
        {
            Instantiate(_visualEffect, owner.transform);
        }
    } 
}
