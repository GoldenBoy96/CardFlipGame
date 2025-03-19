using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] int height;
    [SerializeField] int width;
    [SerializeField] int totalTurn;
    [SerializeField] int scorePerTurn;
    [SerializeField] TMP_InputField heightInputField;
    [SerializeField] TMP_InputField widthInputField;
    [SerializeField] TMP_InputField totalTurnInputField;
    [SerializeField] TMP_InputField scorePerTurnInputField;
    [SerializeField] TMP_InputField levelInputPrefab;
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] TMP_InputField[,] levelInputGameObjects;
    [SerializeField] Sprite cardBack;
    [SerializeField] List<Sprite> cardSprites = new();

    private void OnEnable()
    {
        try
        {
            ReadCurrentLevel();
        }
        catch { }
    }

    private void ReadCurrentLevel()
    {
        level = LevelSaveLoadHelper.LoadLevel();
        heightInputField.text = level.BaseMatrix.GetLength(0).ToString();
        widthInputField.text = level.BaseMatrix.GetLength(1).ToString();
        totalTurnInputField.text = level.TotalTurn.ToString();
        scorePerTurnInputField.text = level.ScorePerTurn.ToString();

        if (levelInputGameObjects != null)
        {
            foreach (var levelInput in levelInputGameObjects)
            {
                if (levelInput != null) PoolingHelper.ReturnObjectToPool(levelInput.gameObject);
            }
        }
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                levelInputGameObjects[i, j] = PoolingHelper.SpawnObject(levelInputPrefab.gameObject, grid.transform, Vector3.zero, Quaternion.identity).GetComponent<TMP_InputField>();
                try
                {
                    levelInputGameObjects[i, j].text = level.BaseMatrix[i, j].ToString();
                    Debug.Log(level.BaseMatrix[i, j]);
                }
                catch
                {
                    levelInputGameObjects[i, j].text = "0";
                }
            }
        }
        GenerateLevelSheet();
    }
    public void GenerateLevelSheet()
    {
        height = Int32.Parse(heightInputField.text);
        width = Int32.Parse(widthInputField.text);
        totalTurn = Int32.Parse(totalTurnInputField.text);
        scorePerTurn = Int32.Parse(scorePerTurnInputField.text);
        grid.constraintCount = width;
        int[,] tmpmatrix = null;

        if (levelInputGameObjects != null)
        {
            tmpmatrix = new int[levelInputGameObjects.GetLength(0), levelInputGameObjects.GetLength(1)];
            for (int i = 0; i < levelInputGameObjects.GetLength(0); i++)
            {
                for (int j = 0; j < levelInputGameObjects.GetLength(1); j++)
                {
                    tmpmatrix[i, j] = Int32.Parse(levelInputGameObjects[i, j].text);
                }
            }
            //LevelSaveLoadHelper.SaveLevel(tmp, "Temporary.txt");
            foreach (var levelInput in levelInputGameObjects)
            {
                levelInput.text = "0";
                PoolingHelper.ReturnObjectToPool(levelInput.gameObject);
            }
        }

        levelInputGameObjects = new TMP_InputField[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                levelInputGameObjects[i, j] = PoolingHelper.SpawnObject(levelInputPrefab.gameObject, grid.transform, Vector3.zero, Quaternion.identity).GetComponent<TMP_InputField>();
                try
                {
                    levelInputGameObjects[i, j].text = tmpmatrix[i, j].ToString();
                }
                catch
                {
                    levelInputGameObjects[i, j].text = "0";
                }
            }
        }
    }

    public void SaveLevel()
    {
        int[,] matrix = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                matrix[i, j] = Int32.Parse(levelInputGameObjects[i, j].text);
            }
        }
        level = new()
        {
            Name = "Level",
            BaseMatrix = matrix,
            ScorePerTurn = scorePerTurn,
            TotalTurn = totalTurn,
        };
        if (level.VerifyLevel())
        {
            LevelSaveLoadHelper.SaveLevel(level);
            UIManager.Instance.OpenTextUI("Save Level Success");
        }
        else
        {
            UIManager.Instance.OpenTextUI("Level is not Playable");
        }
    }

    public void GenerateDefault()
    {
        LevelSaveLoadHelper.GenerateDefaultLevel();
        level = LevelSaveLoadHelper.LoadLevel();
        ReadCurrentLevel();
    }
}
