using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSaveLoadHelper
{
    private static readonly string levelSaveDataPath = $"{Application.persistentDataPath}/FlipCard/Level/";
    private static readonly string fileName = $"Level.txt";
    private static readonly Level defaultLevel = new()
    {
        Name = "Default",
        BaseMatrix = new int[,]
        {
            { 1, 1, 2, 2, 3 },
            { 3, 4, 4, 5, 5 },
            { 6, 6, 7, 7, 8 },
            { 8, 9, 9, 10, 10 },
        },
        ScorePerTurn = 5,
        TotalTurn = 50,
        CardCollection = new()
    };
    public static void SaveLevel(Level level)
    {
        string filePath = $"{levelSaveDataPath}{fileName}";

        JsonHelper.SaveData(level, filePath);
    }

    public static Level LoadLevel()
    {
        string filePath = $"{levelSaveDataPath}{fileName}";
        Level load = JsonHelper.ReadData<Level>(filePath);
        if (load == null) { GenerateDefaultLevel(); }
        load = JsonHelper.ReadData<Level>(filePath);
        return load;
    }

    public static void GenerateDefaultLevel()
    {
        SaveLevel(defaultLevel);
    }
}