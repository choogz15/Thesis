using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

public class NameSync : RealtimeComponent<NameSyncModel>
{
    [SerializeField] private TextMeshPro _textMeshProText;

    protected override void OnRealtimeModelReplaced(NameSyncModel previousModel, NameSyncModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.nameDidChange -= NameDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.name = _textMeshProText.text;
            }

            UpdateName();
            currentModel.nameDidChange += NameDidChange;
        }
    }

    private void NameDidChange(NameSyncModel nameSyncModel, string value)
    {
        UpdateName();
    }

    private void UpdateName()
    {
        _textMeshProText.text = model.name;
    }

    public void SetText(string name)
    {
        model.name = name;
    }
}
