using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayingUIManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab; // Prefab của card
    [SerializeField] Transform cardParent; // Parent của card
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] GameObject[,] cardGameObjects; // Mảng chứa các card
    [SerializeField] List<Sprite> cardSprites; // List chứa các sprite của card
    [SerializeField] Turn currentTurn; // Lượt hiện tại
    [SerializeField] TMP_Text scoreTMP;
    [SerializeField] TMP_Text turnLeftTMP;

    List<Coordinate> registeredCard = new();

    void Awake()
    {
        ObserverHelper.RegisterListener(ObserverConstants.LOADED_GAME,
            (param) =>
            {
                UpdateUIStat((Turn)param[0]);
                GenerateCard(((Turn)param[0]).Matrix);
                currentTurn = (Turn)param[0]; 
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.NORMAL,
            (param) =>
            {
                DoTurn((bool)param[1]);
                currentTurn = (Turn)param[0];
                UpdateUIStat((Turn)param[0]);
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.WIN,
            (param) =>
            {
                DoTurn((bool)param[1]);
                currentTurn = (Turn)param[0];
                UpdateUIStat((Turn)param[0]);
                //Show Win panel here
                Debug.Log("Win");
                UIManager.Instance.OpenWinUI();
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.LOSE,
            (param) =>
            {
                DoTurn((bool)param[1]);
                currentTurn = (Turn)param[0];
                UpdateUIStat((Turn)param[0]);
                //Show Lose panel here
                UIManager.Instance.OpenLoseUI();
            }
            );
    }

    private void OnEnable()
    {
        ObserverHelper.Notify(ObserverConstants.START_GAME);
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
                cardGameObjects[i, j].GetComponent<PlayingCardUI>().SetUpCard(cardSprites[matrix[i, j]], new(i, j), this);
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
                unitWidth = cardGameObjects[i, j].GetComponent<PlayingCardUI>().SetHeightReturnWidth(unitHeight * 0.9f);
            }
        }
        Vector2 newSize = new Vector2(unitWidth * 1.2f, unitHeight);
        gridLayoutGroup.GetComponent<GridLayoutGroup>().spacing = newSize;
    }

    public void RegisterSelectionCard(Coordinate coord)
    {
        if (registeredCard.Count == 0)
        {
            registeredCard.Add(coord);
        }
        else
        {
            registeredCard.Add(coord);
            ObserverHelper.Notify(ObserverConstants.INPUT, registeredCard[0], registeredCard[1]);
        }
    }

    public void DoTurn(bool isFindPair)
    {
        if (isFindPair)
        {
            foreach (var card in registeredCard)
            {
                cardGameObjects[card.X, card.Y].GetComponent<PlayingCardUI>().DisactiveCard();
            }
        }
        else
        {
            foreach (var card in registeredCard)
            {
                cardGameObjects[card.X, card.Y].GetComponent<PlayingCardUI>().FlipCardDown();
            }
        }
        //Debug.Log(currentTurn.CurrentScore);
        registeredCard.Clear();
    }

    private void UpdateUIStat(Turn turn)
    {
        Debug.Log("UpdateUIStat " + turn.TurnLeft);
        scoreTMP.text = $"Score: {turn.CurrentScore}";
        turnLeftTMP.text = $"Turn remain: {turn.TurnLeft}";
    }

    public void Reset()
    {
        ObserverHelper.Notify(ObserverConstants.START_GAME);
    }
}
