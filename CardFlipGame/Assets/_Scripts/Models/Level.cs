using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Level : ICloneable<Level>
{
    [SerializeField] string _name;
    [SerializeField] int scorePerTurn;
    [SerializeField] int totalTurn;
    [SerializeField] int[,] baseMatrix;
    [SerializeField] List<Sprite> cardCollection;
    [SerializeField] Sprite cardBack;

    public string Name { get => _name; set => _name = value; }
    public int ScorePerTurn { get => scorePerTurn; set => scorePerTurn = value; }
    public int TotalTurn { get => totalTurn; set => totalTurn = value; }
    public int[,] BaseMatrix { get => baseMatrix; set => baseMatrix = value; }
    public List<Sprite> CardCollection { get => cardCollection; set => cardCollection = value; }
    public Sprite CardBack { get => cardBack; set => cardBack = value; }

    public bool VerifyLevel()
    {
        bool result = true;
        if (BaseMatrix.GetLength(0) * BaseMatrix.GetLength(1) % 2 == 1) return false;
        //Dictionary<int, List<int>> verifyList = new();
        //foreach (var value in BaseMatrix)
        //{
        //    if (!verifyList.ContainsKey(value))
        //    {
        //        verifyList.Add(value, new());
        //        verifyList[value].Add(value);
        //    }
        //    else
        //    {
        //        verifyList[value].Add(value);
        //    }
        //}
        //foreach (var element in verifyList)
        //{
        //    if (element.Value.Count % 2 == 1)
        //    {
        //        Debug.Log($"Element {element.Key} | {element.Value}");
        //        result = false;
        //        //return false;
        //    }
        //}
        List<int> verifyList = new List<int>();
        for (int i = 0; i < BaseMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < BaseMatrix.GetLength(1); j++)
            {
                verifyList.Add(BaseMatrix[i, j]);
            }
        }

        result = verifyList
           .GroupBy(n => n)
           .All(g => g.Count() % 2 == 0);
        return result;
    }
    public override string ToString()
    {
        return $"{{{nameof(Name)}={Name}, " +
            $"\n{nameof(ScorePerTurn)}={ScorePerTurn.ToString()}, " +
            $"\n{nameof(TotalTurn)}={TotalTurn.ToString()}, " +
            $"\n{nameof(BaseMatrix)}={BaseMatrix}, " +
            $"\n{nameof(CardCollection)}={CardCollection}, " +
            $"\n{nameof(CardBack)}={CardBack}}}";
    }
}
