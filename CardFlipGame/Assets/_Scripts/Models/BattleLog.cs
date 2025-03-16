using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleLog : ICloneable<BattleLog>
{
    [SerializeField] Level level;
    [SerializeField] List<Turn> turns = new();

    public Level Level { get => level; set => level = value; }
    public List<Turn> Turns { get => turns; set => turns = value; }
}
