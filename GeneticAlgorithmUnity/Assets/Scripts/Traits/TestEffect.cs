using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

[CreateAssetMenu]
public class TestEffect : Effect<EntityEventContext>
{
    public override void Trigger(ref EntityEventContext ctx)
    {
        Debug.Log(_message);
    }
}
