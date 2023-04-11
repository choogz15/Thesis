using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class MuteSync : RealtimeComponent<MuteSyncModel>
{
    [SerializeField] private RealtimeAvatarVoice realtimeAvatarVoice;
    [SerializeField] private SpriteRenderer voiceChatRenderer;
    [SerializeField] private Sprite[] voicechatSprites;
    [SerializeField] private bool mute;

    private void Awake()
    {
        realtimeAvatarVoice = GetComponentInChildren<RealtimeAvatarVoice>();
    }

    protected override void OnRealtimeModelReplaced(MuteSyncModel previousModel, MuteSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.muteDidChange -= MuteDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.mute = mute;
            }

            UpdateMute();

            currentModel.muteDidChange += MuteDidChange;
        }
    }

    private void MuteDidChange(MuteSyncModel model, bool value)
    {
        UpdateMute();
    }

    private void UpdateMute()
    {
        mute = model.mute;
        if (mute)
        {
            //Debug.Log("UpdateMute mute");
            realtimeAvatarVoice.mute = true;
            voiceChatRenderer.sprite = voicechatSprites[0];
        }
        else
        {
            //Debug.Log("UpdateMute unmute");
            realtimeAvatarVoice.mute = false;
            voiceChatRenderer.sprite = voicechatSprites[1];
        }
    }

    public void ToggleVoiceChat()
    {
        //Debug.Log("ToggleVoiceChat called");
        if (!realtimeView.isOwnedLocallySelf) return;
        mute = !mute;
        model.mute = mute;
    }
}
