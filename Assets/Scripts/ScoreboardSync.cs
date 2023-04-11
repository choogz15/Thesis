using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

public class ScoreboardSync : RealtimeComponent<ScoreboardModel>
{
    [SerializeField] GameObject incomingCallMenu;


    protected override void OnRealtimeModelReplaced(ScoreboardModel previousModel, ScoreboardModel currentModel)
    {
        currentModel.players.modelAdded += PlayerAddedToScoreboard;
        currentModel.players.modelRemoved += PlayerRemovedFromScoreboard;
    }



    public int GetScore(uint playerId)
    {
        if (!PlayerExistsInDict(playerId)) return 0;
        
        return model.players[playerId].score;
    }

    public void IncrementScore(uint playerId)
    {
        if (!PlayerExistsInDict(playerId))
            AddPlayerToDict(playerId);

        model.players[playerId].score += 1;
    }

    private void AddPlayerToDict(uint playerId)
    {
        UserScoreModel newUserScoreModel = new UserScoreModel();
        newUserScoreModel.score = 0;
        model.players.Add(playerId, newUserScoreModel);
    }

    public void RemovePlayerFromDict(uint playerId)
    {
        model.players.Remove(playerId);
    }

    private bool PlayerExistsInDict(uint playerId)
    {
        try
        {
            UserScoreModel _ = model.players[playerId];
            return true;
        }
        catch
        {
            return false;
        }
    }
    private void PlayerAddedToScoreboard(RealtimeDictionary<UserScoreModel> dictionary, uint key, UserScoreModel model, bool remote)
    {
        Debug.Log("Remote player added to scoreboard");
        incomingCallMenu.SetActive(true);
    }
    private void PlayerRemovedFromScoreboard(RealtimeDictionary<UserScoreModel> dictionary, uint key, UserScoreModel model, bool remote)
    {
        Debug.Log("Remote player removed from scoreboard");
        incomingCallMenu.SetActive(false);
    }



}
