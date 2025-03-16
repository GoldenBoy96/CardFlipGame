using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Sample", menuName = "ScriptableObjects/Sample", order = 0)]
[Serializable]
public class SampleModelSO : ScriptableObject
{
    [SerializeField] private SampleModel data;

    public SampleModel Data => data;
}
