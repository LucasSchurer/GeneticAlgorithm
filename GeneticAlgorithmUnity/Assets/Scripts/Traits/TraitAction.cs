using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Events;

public class TraitAction
{
    private TraitActionSettings _settings;
    private bool _canAct = true;
    private EntityTraitController _traitController;

    public TraitAction(EntityTraitController traitController, TraitActionSettings settings)
    {
        _traitController = traitController;
        _settings = settings;
    }

    public void Action(ref EntityEventContext ctx)
    {
        if (_canAct)
        {
            Debug.Log(_settings.Message);

            _traitController.StartCoroutine(CooldownCoroutine());
        }
    }

    private IEnumerator CooldownCoroutine()
    {
        _canAct = false;

        yield return new WaitForSeconds(_settings.Cooldown);

        _canAct = true;
    }
}
