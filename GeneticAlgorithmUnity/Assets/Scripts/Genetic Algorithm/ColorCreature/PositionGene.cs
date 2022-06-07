using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGene : Gene
{
    public Vector3 position;
    public Vector2 variance;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    public PositionGene(Vector2 variance, Vector2 minBounds, Vector2 maxBounds)
    {
        this.variance = variance;
        this.minBounds = minBounds;
        this.maxBounds = maxBounds;
    }

    public override void Mutate()
    {
        float offsetXRange = Random.Range(-variance.x, variance.x);
        float offsetYRange = Random.Range(-variance.y, variance.y);

        position.x = Mathf.Clamp(position.x + offsetXRange, minBounds.x, maxBounds.x);
        position.y = Mathf.Clamp(position.y + offsetYRange, minBounds.y, maxBounds.y);
    }

    public override void Randomize()
    {
        position.x = Random.Range(minBounds.x, maxBounds.x);
        position.y = Random.Range(minBounds.y, maxBounds.y);
    }
}
