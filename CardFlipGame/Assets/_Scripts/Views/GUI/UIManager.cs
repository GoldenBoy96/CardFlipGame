using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startUI; // UI Start Game
    public GameObject pauseUI; // UI Pause Game
    public GameObject gameOverUI; // UI Game Over

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

}
