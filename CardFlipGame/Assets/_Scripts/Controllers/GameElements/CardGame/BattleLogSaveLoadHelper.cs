﻿using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BattleLogSaveLoadHelper
{
    private static string battleSaveDataPath = $"{Application.persistentDataPath}/FlipCard/BattleLog/";
    public static void SaveBattleLog(BattleLog battleLog)
    {
        if (battleLog.Turns == null || battleLog.Turns.Count == 0)
        {
            Debug.Log("Nothing to save!");
            return;
        }

        string filePath = $"{battleSaveDataPath}BattleLog{battleLog.Level.Name}_{DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss")}.txt";

        JsonHelper.SaveData(battleLog, filePath);
    }

    public static BattleLog LoadBattleLog(string fileName)
    {
        string filePath = $"{battleSaveDataPath}{fileName}";
        return JsonHelper.ReadData<BattleLog>(filePath);
    }

    public static List<string> GetBattleLogNameList()
    {
        List<string> saveNameList = new();
        var info = new DirectoryInfo(battleSaveDataPath);
        var fileInfo = info.GetFiles();

        foreach (var file in fileInfo)
        {
            saveNameList.Add(file.Name);
        }
        return saveNameList;
    }

    public static void DeleteBattleLog(string fileName)
    {
        string filePath = $"{battleSaveDataPath}{fileName}";
        System.IO.File.Delete(filePath);
    }

    public static void OpenExplorer()
    {
        string winPath = $"{Application.persistentDataPath}/FlipCard/BattleLog";
        winPath = winPath.Replace("/", "\\"); 
        Debug.Log(winPath);
        Process.Start("explorer.exe", $"/open, {winPath}");

    }
}