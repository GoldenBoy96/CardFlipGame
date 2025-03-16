using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class SampleGameElementController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SampleModelSO sampleModelSO;


    [Header("Runtime Parameter")]
    [SerializeField] private SampleModel sampleModel;
    [SerializeField] private SampleState currentState;

    private void Awake()
    {
        //Register Observer
        ObserverHelper.RegisterListener(ObserverConstants.DEFAULT, (param) => { SampleBehavior((int)param[0], (string)param[1]); } );
        ObserverHelper.Notify(ObserverConstants.DEFAULT, new[] { "1", "Hello" });
    }

    private void OnEnable()
    {
        sampleModel = ((ICloneable<SampleModel>)sampleModelSO.Data).CloneSelf();
    }

    private void Update()
    {
        UpdateOnState();
    }

    public void SampleBehavior(int num, string name)
    {
        //TO DO: Any thing
        Debug.Log(num + name);
    }

    #region State Machine
    public void SwitchToState(SampleState incomingState)
    {
        if (incomingState == currentState) return;
        switch (currentState)
        {
            case SampleState.Default:
                ExitState_Default();
                break;
        }

        currentState = incomingState;

        switch (incomingState)
        {
            case SampleState.Default:
                EnterState_Default();
                break;
        }
    }

    protected void UpdateOnState()
    {
        switch (currentState)
        {
            case SampleState.Default:
                UpdateState_Default();
                break;
        }
    }

    #region State Default
    protected virtual void EnterState_Default()
    { 
        
    }
    protected virtual void UpdateState_Default()
    { 
        
    }
    protected virtual void ExitState_Default()
    { 
        
    }
    #endregion

    #endregion
}
