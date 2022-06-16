using System;
using System.Threading;

public static class StaticRandom
{
    static int seed = Environment.TickCount;

    static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

    public static int RandomInt(int min, int max)
    {
        return random.Value.Next(min, max);
    }

    public static float RandomFloat(float min, float max)
    {
        return (float)random.Value.NextDouble() * (max - min) + min;
    }
}
