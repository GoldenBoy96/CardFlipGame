using System.Collections;
using System.Collections.Generic;
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

    List<Coordinate> registeredCard = new();

    void Awake()
    {
        ObserverHelper.RegisterListener(ObserverConstants.LOADED_GAME,
            (param) =>
            {
                GenerateCard(((Turn)param[0]).Matrix);
                currentTurn = (Turn)param[0];
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.NORMAL,
            (param) =>
            {
                DoTurn((bool)param[1]);
                currentTurn = (Turn)param[0];
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.WIN,
            (param) =>
            {
                DoTurn((bool)param[1]);
                currentTurn = (Turn)param[0];
                //Show Win panel here
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.LOSE,
            (param) =>
            {
                DoTurn((bool)param[1]);
                currentTurn = (Turn)param[0];
                //Show Lose panel here
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
            }
        }

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = matrix.GetLength(1);
        //TO DO: Làm reponsive cho gridLayoutGroup
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
        Debug.Log(currentTurn.CurrentScore);
        registeredCard.Clear();
    }
}
