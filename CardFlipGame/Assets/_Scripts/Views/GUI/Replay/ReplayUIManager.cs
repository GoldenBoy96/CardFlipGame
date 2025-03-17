using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplayingUIManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab; // Prefab của card
    [SerializeField] Transform cardParent; // Parent của card
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] GameObject[,] cardGameObjects; // Mảng chứa các card
    [SerializeField] List<Sprite> cardSprites; // List chứa các sprite của card

    [SerializeField] BattleLog battleLog;
    [SerializeField] Turn currentTurn; // Lượt hiện tại   
    [SerializeField] float delayTimeBetweenTurn = 1f;

    [SerializeField] TMP_Text scoreTMP;
    [SerializeField] TMP_Text turnLeftTMP;

    List<Coordinate> registeredCard = new();

    void Awake()
    {
    }

    private void OnEnable()
    {
        StartReplay();
    }

    public void StartReplay()
    {
        LoadBattleLog();
        currentTurn = battleLog.Turns[0];
        GenerateCard(currentTurn.Matrix);
        StartCoroutine(WaitAndDoTurn(0));
    }

    private void LoadBattleLog()
    {
        var battleLogName = "BattleLogDefault_17-03-2025_04-49-57";
        battleLog = BattleLogSaveLoadHelper.LoadBattleLog(battleLogName);
    }
    IEnumerator WaitAndDoTurn(int turnIndex)
    {
        currentTurn = battleLog.Turns[turnIndex];
        Coordinate firstCardCoord = battleLog.Turns[turnIndex].InputPair.FirstCoord;
        Coordinate secondCardCoord = battleLog.Turns[turnIndex].InputPair.SecondCoord;
        cardGameObjects[firstCardCoord.X, firstCardCoord.Y].GetComponent<ReplayCardUI>().FlipCardUp();
        yield return new WaitForSeconds(delayTimeBetweenTurn);
        cardGameObjects[secondCardCoord.X, secondCardCoord.Y].GetComponent<ReplayCardUI>().FlipCardUp();
        yield return new WaitForSeconds(delayTimeBetweenTurn);
        Debug.Log(currentTurn.GameStatus);
        switch (currentTurn.GameStatus)
        {
            case GameStatus.FindPair:
                cardGameObjects[firstCardCoord.X, firstCardCoord.Y].GetComponent<ReplayCardUI>().DisactiveCard();
                cardGameObjects[secondCardCoord.X, secondCardCoord.Y].GetComponent<ReplayCardUI>().DisactiveCard();
                break;
            case GameStatus.Normal:
                cardGameObjects[firstCardCoord.X, firstCardCoord.Y].GetComponent<ReplayCardUI>().FlipCardDown();
                cardGameObjects[secondCardCoord.X, secondCardCoord.Y].GetComponent<ReplayCardUI>().FlipCardDown();
                break;
            case GameStatus.Win:
                cardGameObjects[firstCardCoord.X, firstCardCoord.Y].GetComponent<ReplayCardUI>().DisactiveCard();
                cardGameObjects[secondCardCoord.X, secondCardCoord.Y].GetComponent<ReplayCardUI>().DisactiveCard();
                //Show Win Panel
                break;
            case GameStatus.Lose:
                cardGameObjects[firstCardCoord.X, firstCardCoord.Y].GetComponent<ReplayCardUI>().DisactiveCard();
                cardGameObjects[secondCardCoord.X, secondCardCoord.Y].GetComponent<ReplayCardUI>().DisactiveCard();
                //Show Lose Panel
                break;
        }
        if (turnIndex < battleLog.Turns.Count - 1)
        {
            var flipCardTime = cardPrefab.GetComponent<ReplayCardUI>().FlipCardTime;
            yield return new WaitForSeconds(flipCardTime);
            UpdateUIStat(currentTurn);
            StartCoroutine(WaitAndDoTurn(turnIndex + 1));
        }

    }
    public void GenerateCard(int[,] matrix)
    {
        foreach (Transform child in cardParent)
        {
            Debug.Log(child);
            PoolingHelper.ReturnObjectToPool(child.gameObject);
        }
        // Generate card
        cardGameObjects = new GameObject[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                cardGameObjects[i, j] = PoolingHelper.SpawnObject(cardPrefab, cardParent, Vector3.zero, Quaternion.identity);
                cardGameObjects[i, j].GetComponent<ReplayCardUI>().SetUpCard(cardSprites[matrix[i, j]], new(i, j), this);
            }
        }

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = matrix.GetLength(1);

        //Reponsive cho gridLayoutGroup 
        var totalHeight = cardParent.GetComponent<RectTransform>().rect.height;
        var unitHeight = totalHeight / (matrix.GetLength(0));
        Debug.Log(totalHeight + " | " + matrix.GetLength(0));
        float unitWidth = 0;
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                unitWidth = cardGameObjects[i, j].GetComponent<ReplayCardUI>().SetHeightReturnWidth(unitHeight * 0.9f);
            }
        }
        Vector2 newSize = new Vector2(unitWidth * 1.2f, unitHeight);
        gridLayoutGroup.GetComponent<GridLayoutGroup>().spacing = newSize;
    }
    private void UpdateUIStat(Turn turn)
    {
        scoreTMP.text = $"Score: {turn.CurrentScore}";
        turnLeftTMP.text = $"Turn remain: {turn.TurnLeft}";
    }

}
