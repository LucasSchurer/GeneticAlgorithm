using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

[CreateAssetMenu]
public class Trait : ScriptableObject
{
    [System.Serializable]
    public struct Definition
    {
        public TraitActionSettings settings;
        public EntityEventType eventType;
        public EventExecutionOrder eventOrder;
    }

    [SerializeField]
    protected Definition[] _definitions;

    public Definition[] Definitions => _definitions;
}
