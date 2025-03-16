using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCardGameController : MonoBehaviour
{
    [SerializeField] Level level;

    private void Start()
    {
        LevelSaveLoadHelper.GenerateDefaultLevel();
    }
    public void StartGame()
    {
        level = LevelSaveLoadHelper.LoadLevel();
    }
}
