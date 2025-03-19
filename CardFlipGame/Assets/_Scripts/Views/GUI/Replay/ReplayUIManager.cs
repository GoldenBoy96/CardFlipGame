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

    [Header("Replay Control Panel")]
    [SerializeField] TMP_Dropdown replayListDropdown;
    [SerializeField] Scrollbar timeLineScrollBar;
    int currentTurnIndex = 0;
    bool isPlaying = false;
    List<Coroutine> replayCoroutines = new List<Coroutine>();

    List<Coordinate> registeredCard = new();

    void Awake()
    {
    }

    private void OnEnable()
    {
        SetReplayNameList();
    }

    private void StartReplay()
    {
        currentTurn = battleLog.Turns[currentTurnIndex];
        if (currentTurnIndex == 0)
        {
            GenerateCard(battleLog.Level.BaseMatrix);
        }
        else
        {
            GenerateCard(currentTurn.Matrix);
        }
        replayCoroutines.Add(StartCoroutine(WaitAndDoTurn(currentTurnIndex)));
    }

    IEnumerator WaitAndDoTurn(int turnIndex)
    {
        var flipCardTime = cardPrefab.GetComponent<ReplayCardUI>().FlipCardTime;
        yield return new WaitForSeconds(flipCardTime);
        currentTurn = battleLog.Turns[turnIndex];
        timeLineScrollBar.value = (turnIndex + 1) * 1f / battleLog.Turns.Count;

        Coordinate firstCardCoord = battleLog.Turns[turnIndex].InputPair.FirstCoord;
        Coordinate secondCardCoord = battleLog.Turns[turnIndex].InputPair.SecondCoord;
        cardGameObjects[firstCardCoord.X, firstCardCoord.Y].GetComponent<ReplayCardUI>().FlipCardUp();
        yield return new WaitForSeconds(delayTimeBetweenTurn);
        cardGameObjects[secondCardCoord.X, secondCardCoord.Y].GetComponent<ReplayCardUI>().FlipCardUp();
        yield return new WaitForSeconds(delayTimeBetweenTurn);
        //Debug.Log(currentTurn.GameStatus);
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
            UpdateUIStat(currentTurn);

            replayCoroutines.Add(StartCoroutine(WaitAndDoTurn(turnIndex + 1)));
        }
    }
    public void GenerateCard(int[,] matrix)
    {
        foreach (Transform child in cardParent)
        {
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
                if (matrix[i, j] == 0) cardGameObjects[i, j].GetComponent<ReplayCardUI>().DisactiveCard();
            }
        }

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = matrix.GetLength(1);

        //Reponsive cho gridLayoutGroup 
        var totalHeight = cardParent.GetComponent<RectTransform>().rect.height;
        var unitHeight = totalHeight / (matrix.GetLength(0));
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

    #region Control Replay

    public void LoadBattleLog()
    {
        Pause();
        battleLog = BattleLogSaveLoadHelper.LoadBattleLog(replayListDropdown.options[replayListDropdown.value].text);
        currentTurnIndex = 0;
        timeLineScrollBar.value = 0;
        GenerateCard(battleLog.Level.BaseMatrix);
    }
    public void SetReplayNameList()
    {
        int selected = replayListDropdown.value;
        replayListDropdown.options.Clear();
        var options = BattleLogSaveLoadHelper.GetBattleLogNameList();
        for (int i = 0; i < options.Count; i++)
        {
            replayListDropdown.options.Add(new TMP_Dropdown.OptionData(options[i]));
        }
        if (selected >= replayListDropdown.options.Count)
        {
            selected = 0;
        }
        replayListDropdown.value = selected;
    }

    public void SetTurnFromSlider()
    {
        currentTurnIndex = (int)(timeLineScrollBar.value * battleLog.Turns.Count);
        GenerateCard(battleLog.Turns[currentTurnIndex].Matrix);
        timeLineScrollBar.value = currentTurnIndex * 1f / battleLog.Turns.Count;
    }
    public void Play()
    {
        if (isPlaying)
        {
            Pause();
        }
        else
        {
            battleLog = BattleLogSaveLoadHelper.LoadBattleLog(replayListDropdown.options[replayListDropdown.value].text);
            Debug.Log(currentTurnIndex);
            timeLineScrollBar.size = 1f / (battleLog.Turns.Count);
            StartReplay();
            isPlaying = true;
        }
    }

    public void Pause()
    {
        foreach (var coroutine in replayCoroutines)
        {
            StopCoroutine(coroutine);
        }
        replayCoroutines.Clear();
        isPlaying = false;
    }

    public void Delete()
    {
        //TO DO: Add confirm panel
    }

    public void OpenFolder()
    {
        BattleLogSaveLoadHelper.OpenExplorer();
    }
    #endregion
}
