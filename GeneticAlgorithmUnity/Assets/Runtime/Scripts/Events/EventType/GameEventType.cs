using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public enum GameEventType
    {
        OnWaveEnd,
        OnWaveStart,
        OnApplicationQuit,
        OnGivePlayerTraits,
        OnPause,
        OnResume,
        OnGameOver,
    }
}
