using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Events;

public abstract class TraitAction
{
    protected TraitActionSettings _settings;
    protected bool _canAct = true;
    protected EntityTraitController _traitController;

    public TraitAction(EntityTraitController traitController, TraitActionSettings settings)
    {
        _traitController = traitController;
        _settings = settings;
    }

    public abstract void Action(ref EntityEventContext ctx);
    protected IEnumerator CooldownCoroutine()
    {
        _canAct = false;

        yield return new WaitForSeconds(_settings.Cooldown);

        _canAct = true;
    }
}
