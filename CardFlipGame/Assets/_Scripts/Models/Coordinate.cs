using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Coordinate : ICloneable<Coordinate>
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    public Coordinate()
    {
    }

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public override string ToString()
    {
        return $"{{{nameof(X)}={X.ToString()}, {nameof(Y)}={Y.ToString()}}}";
    }
}
