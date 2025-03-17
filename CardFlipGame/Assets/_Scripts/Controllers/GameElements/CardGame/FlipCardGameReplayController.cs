using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCardGameReplayController : MonoBehaviour
{
    [SerializeField] BattleLog battleLog;

    public List<string> LoadBattleLogName()
    {
        return BattleLogSaveLoadHelper.GetBattleLogNameList();
    }

    public BattleLog GetBattleLog(string name)
    {
        return BattleLogSaveLoadHelper.LoadBattleLog(name);
    }
}
