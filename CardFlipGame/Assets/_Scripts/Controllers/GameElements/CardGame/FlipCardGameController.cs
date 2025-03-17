using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCardGameController : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] BattleLog battleLog;
    [SerializeField] Turn currentTurn;

    private void Awake()
    {
        ObserverHelper.RegisterListener(ObserverConstants.START_GAME,
            (param) =>
            {
                StartGame();
            }
            );
        ObserverHelper.RegisterListener(ObserverConstants.INPUT,
            (param) =>
            {
                DoTurn((Coordinate)param[0], (Coordinate)param[1]);
            }
            );
    }
    private void Start()
    {
        StartGame();
    }
    public void StartGame()
    {
        level = LevelSaveLoadHelper.LoadLevel();
        currentTurn = new Turn()
        {
            CurrentScore = 0,
            Matrix = level.BaseMatrix,
            TurnLeft = level.TotalTurn,
            GameStatus = GameStatus.Normal
        };

        battleLog = new BattleLog
        {
            Level = level
        };
        ObserverHelper.Notify(ObserverConstants.LOADED_GAME, currentTurn);
    }

    public void DoTurn(Coordinate coord1, Coordinate coord2)
    {
        currentTurn = ((ICloneable<Turn>)currentTurn).CloneSelf();
        currentTurn.GameStatus = GameStatus.Normal;

        currentTurn.TurnLeft--;
        currentTurn.InputPair = new(coord1, coord2);

        bool isFindPair = Validate(coord1, coord2);
        if (isFindPair)
        {
            currentTurn.Matrix[coord1.X, coord1.Y] = 0;
            currentTurn.Matrix[coord2.X, coord2.Y] = 0;
            currentTurn.CurrentScore += battleLog.Level.ScorePerTurn;
        }

        currentTurn.GameStatus = CheckGameStatus();
        switch (currentTurn.GameStatus)
        {
            case GameStatus.Normal:
                ObserverHelper.Notify(ObserverConstants.NORMAL, currentTurn, isFindPair);
                break;
            case GameStatus.Win:
                //Notify Win
                ObserverHelper.Notify(ObserverConstants.WIN, currentTurn, isFindPair);
                break;
            case GameStatus.Lose:
                //Notify Lose
                ObserverHelper.Notify(ObserverConstants.LOSE, currentTurn, isFindPair);
                Debug.Log("Lose");
                break;
        }

        if (isFindPair && currentTurn.GameStatus == GameStatus.Normal) currentTurn.GameStatus = GameStatus.FindPair;
        battleLog.Turns.Add(currentTurn);
        if (currentTurn.GameStatus == GameStatus.Win || currentTurn.GameStatus == GameStatus.Lose)
        {
            BattleLogSaveLoadHelper.SaveBattleLog(battleLog);
        }
    }

    public bool Validate(Coordinate coord1, Coordinate coord2)
    {
        //Debug.Log(coord1);
        //Debug.Log(currentTurn.Matrix);
        //Debug.Log(currentTurn.Matrix[coord1.X, coord1.Y]);
        if (currentTurn.Matrix[coord1.X, coord1.Y] == currentTurn.Matrix[coord2.X, coord2.Y])
        {
            return true;
        }
        return false;
    }

    public GameStatus CheckGameStatus()
    {
        bool isNoCardLeft = true;
        foreach (var value in currentTurn.Matrix)
        {
            if (value != 0)
            {
                isNoCardLeft = false;
            }
        }
        if (currentTurn.TurnLeft <= 0)
        {
            if (isNoCardLeft)
            {
                return GameStatus.Win;
            }
            else
            {
                return GameStatus.Lose;
            }
        }
        else
        {
            if (isNoCardLeft)
            {
                return GameStatus.Win;
            }
        }
        return GameStatus.Normal;
    }

}
