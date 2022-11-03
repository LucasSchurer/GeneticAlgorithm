using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Events;

public abstract class TraitAction : ScriptableObject
{
    [SerializeField]
    protected float _cooldown;
    [SerializeField]
    protected string _message;

    protected bool _canAct = true;
    protected EntityTraitController _traitController;

    public abstract void Action(ref EntityEventContext ctx);
    protected abstract IEnumerator CooldownCoroutine();
}
