using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level : ICloneable<Level>
{
    [SerializeField] string _name;
    [SerializeField] int scorePerTurn;
    [SerializeField] int[,] baseMatrix;
    [SerializeField] List<SpriteRenderer> cardCollection;
    [SerializeField] SpriteRenderer cardBack;

    public string Name { get => _name; set => _name = value; }
    public int ScorePerTurn { get => scorePerTurn; set => scorePerTurn = value; }
    public int[,] BaseMatrix { get => baseMatrix; set => baseMatrix = value; }
    public List<SpriteRenderer> CardCollection { get => cardCollection; set => cardCollection = value; }
    public SpriteRenderer CardBack { get => cardBack; set => cardBack = value; }
}
