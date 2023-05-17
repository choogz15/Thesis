using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;

public class TestSync : RealtimeComponent<TestModel>
{
    public RealtimeAvatarVoice voice;
    public VoiceChatDictionary voiceChatDictionary;

    public Button callButton;

    private void Awake()
    {
        voice = GetComponentInChildren<RealtimeAvatarVoice>();
        voiceChatDictionary = GameObject.Find("VoiceChatDictionary").GetComponent<VoiceChatDictionary>();

    }

}
