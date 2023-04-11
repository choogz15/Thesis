using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerSync : RealtimeComponent<PlayerSyncModel>
{
    [SerializeField] private bool isRinging = false;


    protected override void OnRealtimeModelReplaced(PlayerSyncModel previousModel, PlayerSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.isRingingDidChange -= IsRingingDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.isRinging = isRinging;
            }

            UpdateIsRinging();

            currentModel.isRingingDidChange += IsRingingDidChange;
        }
    }

    private void IsRingingDidChange(PlayerSyncModel model, bool value)
    {
        UpdateIsRinging();
    }

    private void UpdateIsRinging()
    {
        isRinging = model.isRinging;

    }

    public void setIsRinging(bool isRinging)
    {
        Debug.Log("setIsRinging called with argument: " + isRinging);
        model.isRinging = isRinging;
    }

}
