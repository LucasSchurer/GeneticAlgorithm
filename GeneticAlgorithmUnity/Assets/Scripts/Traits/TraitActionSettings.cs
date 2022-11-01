using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Events;

[CreateAssetMenu]
public class TraitActionSettings : ScriptableObject
{
    [SerializeField]
    protected float _cooldown;
    [SerializeField]
    protected string _message;
    [SerializeField]
    protected TraitAction _action;

    public float Cooldown => _cooldown;
    public string Message => _message;
}
