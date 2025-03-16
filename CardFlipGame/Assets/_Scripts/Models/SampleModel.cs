using System;
using UnityEngine;

[Serializable]
public class SampleModel : ICloneable<SampleModel>
{
    [SerializeField]private string sampleData = "DEFAULT";

    public string SampleData { get => sampleData; set => sampleData = value; }
}