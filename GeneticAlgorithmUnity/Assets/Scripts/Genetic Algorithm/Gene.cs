using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gene
{
    /// <summary>
    /// Method called when the creature suffers a mutation.
    /// </summary>
    public abstract void Mutate();

    public abstract void Randomize();
}
