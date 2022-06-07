using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGene : Gene
{
    public Vector3 position;
    public Vector3 variance;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    public PositionGene(Vector3 variance, Vector3 minBounds, Vector3 maxBounds)
    {
        this.variance = variance;
        this.minBounds = minBounds;
        this.maxBounds = maxBounds;
    }

    public override void Mutate()
    {
        float offsetXRange = Random.Range(-variance.x, variance.x);
        float offsetYRange = Random.Range(-variance.y, variance.y);
        float offsetZRange = Random.Range(-variance.z, variance.z);

        position.x = Mathf.Clamp(position.x + offsetXRange, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y + offsetYRange, minBounds.y, maxBounds.y);
        position.z = Mathf.Clamp(position.z + offsetZRange, minBounds.z, maxBounds.z);
    }

    public override void Randomize()
    {
        position.x = Random.Range(minBounds.x, maxBounds.x);
        position.y = Random.Range(minBounds.y, maxBounds.y);
        position.z = Random.Range(minBounds.z, maxBounds.z);
    }
}
