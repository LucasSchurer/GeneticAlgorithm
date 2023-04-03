using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Game.GA
{
    [DataContract(Name = "Gene", Namespace = "")]
    public abstract class Gene
    {
        /// <summary>
        /// Method called when the creature suffers a mutation.
        /// </summary>
        public abstract void Mutate();

        public abstract void Randomize();

        public abstract Gene Copy();
        public abstract void Apply(CreatureController creature);
    } 
}
