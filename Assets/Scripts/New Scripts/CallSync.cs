using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class CallSync : RealtimeComponent<CallSyncModel>
{
    public GameObject dialingIndicator;

    protected override void OnRealtimeModelReplaced(CallSyncModel previousModel, CallSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.dialingPlayerDidChange -= DialingPlayerDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
                currentModel.dialingPlayer = 0;

            UpdateDialingIndicator();

            currentModel.dialingPlayerDidChange += DialingPlayerDidChange;
        }
    }

    private void DialingPlayerDidChange(CallSyncModel model, int value)
    {
        UpdateDialingIndicator();
    }

    private void UpdateDialingIndicator()
    {
        bool showIndicator = model.dialingPlayer == 0 ? false : true;
        dialingIndicator.SetActive(showIndicator);
    }

    public void CallOtherPlayer()
    {
        model.dialingPlayer = 1;
    }
}
