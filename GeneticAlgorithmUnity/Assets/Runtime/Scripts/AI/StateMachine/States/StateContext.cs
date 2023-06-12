using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    /// <summary>
    /// Used by a State Machine to allow communication between two states (the last and current state)
    /// </summary>
    public class StateContext
    {
        [SerializeField]
        private TargetPacket _targetPacket;

        public TargetPacket Target { get => _targetPacket; set => _targetPacket = value; }

        public class TargetPacket
        {
            private Transform _target;

            public Transform Target { get => _target; set => _target = value; }
        }
    }
}
