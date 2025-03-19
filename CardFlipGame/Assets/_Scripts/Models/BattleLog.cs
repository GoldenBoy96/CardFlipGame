using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleLog : ICloneable<BattleLog>
{
    [SerializeField] string _name;
    [SerializeField] Level level;
    [SerializeField] List<Turn> turns = new();

    public string Name { get => _name; set => _name = value; }
    public Level Level { get => level; set => level = value; }
    public List<Turn> Turns { get => turns; set => turns = value; }

    public override bool Equals(object obj)
    {
        return obj is BattleLog log &&
               Name == log.Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }

    public override string ToString()
    {
        return $"{{{nameof(Name)}={Name}, {nameof(Level)}={Level}, {nameof(Turns)}={Turns}}}";
    }
}
