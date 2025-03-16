using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Turn : ICloneable<Turn>
{
    [SerializeField] PairCoord inputPair;
    [SerializeField] int currentScore;
    [SerializeField] int[,] matrix;

    public PairCoord InputPair { get => inputPair; set => inputPair = value; }
    public int CurrentScore { get => currentScore; set => currentScore = value; }
    public int[,] Matrix { get => matrix; set => matrix = value; }
}


[Serializable]
public struct PairCoord
{
    [SerializeField] Coordinate firstCoord;
    [SerializeField] Coordinate secondCoord;

    public Coordinate FirstCoord { get => firstCoord; set => firstCoord = value; }
    public Coordinate SecondCoord { get => secondCoord; set => secondCoord = value; }
}
