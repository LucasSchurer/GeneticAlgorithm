using System.Runtime.Serialization;

namespace Game.Traits
{
    public enum TraitIdentifier
    {
        None,
        Default,
        MoreExplosiveDamageLessOverallDamage,
        MoreOverallDamageLessExplosiveDamage,
        MoreRateOfFireLessMovementSpeed,
        MoreHealth,
        MoreRateOfFire,
        MoreDamageMultiplier,
        MoreProjectileAmount,
        MoreDamageReductionLessOverallDamage,
        LessDamageReductionMoreDamage,
        LessDamageReductionMoreHealth,
    } 
}
