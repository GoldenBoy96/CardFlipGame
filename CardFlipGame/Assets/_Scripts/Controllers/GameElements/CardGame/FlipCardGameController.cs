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
            TurnLeft = 0,
            GameStatus = GameStatus.Playing
        };

        battleLog = new BattleLog
        {
            Level = level
        };
        battleLog.Turns.Add(currentTurn);
        ObserverHelper.Notify(ObserverConstants.PLAYING, currentTurn);
    }

    public void DoTurn(Coordinate coord1, Coordinate coord2)
    {
        currentTurn = ((ICloneable<Turn>)currentTurn).CloneSelf();
        currentTurn.TurnLeft--;
        currentTurn.InputPair = new(coord1, coord2);

        if (Validate(coord1, coord2))
        {
            currentTurn.Matrix[coord1.X, coord1.Y] = 0;
            currentTurn.Matrix[coord2.X, coord2.Y] = 1;
            currentTurn.CurrentScore += battleLog.Level.ScorePerTurn;
        }

        currentTurn.GameStatus = CheckGameStatus();
        switch (currentTurn.GameStatus)
        {
            case GameStatus.Playing:
                ObserverHelper.Notify(ObserverConstants.PLAYING, currentTurn);
                break;
            case GameStatus.Win:
                //Notify Win
                ObserverHelper.Notify(ObserverConstants.WIN, currentTurn);
                Debug.Log("Win");
                BattleLogSaveLoadHelper.SaveBattleLog(battleLog);
                break;
            case GameStatus.Lose:
                //Notify Lose
                ObserverHelper.Notify(ObserverConstants.LOSE, currentTurn);
                Debug.Log("Lose");
                BattleLogSaveLoadHelper.SaveBattleLog(battleLog);
                break;
        }

        battleLog.Turns.Add(currentTurn);
    }

    public bool Validate(Coordinate coord1, Coordinate coord2)
    {
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
        return GameStatus.Playing;
    }

}
