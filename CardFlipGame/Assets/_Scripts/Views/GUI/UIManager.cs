using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startUI; // UI Start Game
    public GameObject pauseUI; // UI Pause Game
    public GameObject gameOverUI; // UI Game Over

    public GameObject cardPrefab; // Prefab của card
    public Transform cardParent; // Parent của card

    public GameObject[,] cardGameObjects; // Mảng chứa các card

    public List<Sprite> cardSprites; // List chứa các sprite của card

    public Turn currentTurn; // Lượt hiện tại   

    void Awake()
    {
        ObserverHelper.RegisterListener(ObserverConstants.START_GAME,
            (param) =>
            {
                GenerateCard(((Turn)param[0]).Matrix);
                currentTurn = (Turn)param[0];
            }
            );
    }
    public void UIStart()
    {
        startUI.SetActive(true); // Hiển thị UI Start Game
    }

    public void OpenUIPause()
    {
        pauseUI.SetActive(true); // Hiển thị UI Pause Game
    }

    public void CloseUIPause()
    {
        pauseUI.SetActive(false); // Ẩn UI Pause Game
    }

    public void UIGameOver()
    {
        gameOverUI.SetActive(true); // Hiển thị UI Game Over
    }

    public void GenerateCard(int[,] matrix)
    {
        foreach (Transform child in cardParent)
        {
            PoolingHelper.ReturnObjectToPool(child.gameObject);
        }
        // Generate card
        int cardIndex = 0;
        cardGameObjects = new GameObject[matrix.GetLength(0), matrix.GetLength(1)];
        cardSprites = cardSprites.Concat(cardSprites).ToList();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                cardGameObjects[i, j] = PoolingHelper.SpawnObject(cardPrefab, cardParent, Vector3.zero, Quaternion.identity);
                cardGameObjects[i, j].GetComponentInChildren<Image>().sprite = cardSprites[matrix[i, j]];
            }
        }
    }

}
