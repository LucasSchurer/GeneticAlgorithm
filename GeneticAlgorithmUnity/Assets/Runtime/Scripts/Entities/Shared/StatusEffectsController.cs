using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class StatusEffectsController : MonoBehaviour
    {
        private Dictionary<StatusEffectType, StatusEffect> _effects = new Dictionary<StatusEffectType, StatusEffect>();

        public bool HasStatusEffect(StatusEffectType type)
        {
            return _effects.ContainsKey(type);
        }

        public void AddStatusEffect(StatusEffectType type, float strength)
        {
            _effects.TryAdd(type, new StatusEffect(type, strength));
        }
    } 
}
