using UnityEngine;
using Game.Events;

namespace Game.Traits.Effects
{
    public static class EffectsHelper
    {
        public static bool TryGetTarget(TargetType type, EventContext ctx, out GameObject target)
        {
            switch (type)
            {
                case TargetType.Self:
                    if (ctx.Owner)
                    {
                        target = ctx.Owner;
                        return true;
                    } else
                    {
                        target = null;
                        return false;
                    }
                case TargetType.Other:
                    if (ctx.Other)
                    {
                        target = ctx.Other;
                        return true;
                    }
                    else
                    {
                        target = null;
                        return false;
                    }
                default:
                    target = null;
                    return false;
            }
        }
    } 
}
