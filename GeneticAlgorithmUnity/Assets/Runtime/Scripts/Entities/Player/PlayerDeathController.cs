using Game.Entities.Shared;
using Game.Events;
using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerDeathController : DeathController
    {
        protected override void OnDeath(ref EntityEventContext ctx)
        {
            GameManager.Instance.GameOver();
        }
    } 
}
