using Game;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTraitAction : TraitAction
{
    public TestTraitAction(EntityTraitController traitController, TraitActionSettings settings) : base(traitController, settings)
    {
    }

    public override void Action(ref EntityEventContext ctx)
    {
        if (_canAct)
        {
            Debug.Log(_settings.Message);

            _traitController.StartCoroutine(CooldownCoroutine());
        }
    }
}
