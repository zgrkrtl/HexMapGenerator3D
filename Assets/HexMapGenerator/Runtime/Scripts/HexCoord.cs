using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HexCoord
{
    public int q;
    public int r;

    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public int s => -q - r;

    public static readonly HexCoord[] directions = new[]
    {
        new HexCoord(+1, 0),
        new HexCoord(+1, -1),
        new HexCoord(0, -1),
        new HexCoord(-1, 0),
        new HexCoord(-1, +1),
        new HexCoord(0, +1)
    };

    public HexCoord Neighbor(int direction)
    {
        return new HexCoord(q + directions[direction].q, r + directions[direction].r);
    }
    public IEnumerable<HexCoord> AllNeighbors()
    {
        for (int i = 0; i < 6; i++)
            yield return Neighbor(i);
    }

    public int Distance(HexCoord other)
    {
        return Mathf.Max(
            Mathf.Abs(q - other.q),
            Mathf.Abs(r - other.r),
            Mathf.Abs(s - other.s)
        );
    }

    public Vector3 ToWorld(float radius, Vector3 origin)
    {
        float x = radius * Mathf.Sqrt(3f) * (q + r * 0.5f);
        float z = radius * 1.5f * r;
        return origin + new Vector3(x, 0, z);
    }

    public override string ToString() => $"({q}, {r})";

    public static HexCoord operator +(HexCoord a, HexCoord b)
        => new HexCoord(a.q + b.q, a.r + b.r);

    public static bool operator ==(HexCoord a, HexCoord b)
        => a.q == b.q && a.r == b.r;

    public static bool operator !=(HexCoord a, HexCoord b)
        => !(a == b);

    public override bool Equals(object obj)
        => obj is HexCoord other && this == other;

    public override int GetHashCode()
        => (q, r).GetHashCode();
}
