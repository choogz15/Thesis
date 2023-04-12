using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using System;

public class PlayerTest : RealtimeComponent<PlayerTestModel>
{
    public int playerID;


    [RealtimeProperty(1, true, true)] private bool _playerID;
    [RealtimeProperty(2, true, true)] private int _dialingPlayer;
    [RealtimeProperty(3, true, true)] private int _talkingWithPlayer;
    [RealtimeProperty(4, true, true)] private int _terminatedPlayer;
    [RealtimeProperty(5, true, true)] private float _scaleFactor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnRealtimeModelReplaced(PlayerTestModel previousModel, PlayerTestModel currentModel)
    {
        if(previousModel != null)
        {
            previousModel.playerIDDidChange -= PlayerIDDidChange;
            previousModel.dialingPlayerDidChange -= DialingPlayerDidChange;
            previousModel.talkingWithPlayerDidChange -= TalkingWithPlayerDidChange;
            previousModel.terminatedPlayerDidChange -= TerminatedPlayerDidChange;
            previousModel.scaleFactorDidChange -= ScaleFactorDidChange;
        }

        if(currentModel != null)
        {

        }

    }

    private void ScaleFactorDidChange(PlayerTestModel model, float value)
    {
        throw new NotImplementedException();
    }

    private void TerminatedPlayerDidChange(PlayerTestModel model, int value)
    {
        throw new NotImplementedException();
    }

    private void TalkingWithPlayerDidChange(PlayerTestModel model, int value)
    {
        throw new NotImplementedException();
    }

    private void DialingPlayerDidChange(PlayerTestModel model, int value)
    {
        throw new NotImplementedException();
    }

    private void PlayerIDDidChange(PlayerTestModel model, bool value)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
