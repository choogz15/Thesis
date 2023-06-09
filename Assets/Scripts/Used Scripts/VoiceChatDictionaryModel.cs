using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class VoiceChatDictionaryModel
{
    [RealtimeProperty(1, true, false)] private RealtimeDictionary<VoiceChatModel> _players;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class VoiceChatDictionaryModel : RealtimeModel {
    public Normal.Realtime.Serialization.RealtimeDictionary<VoiceChatModel> players {
        get => _players;
    }
    
    public enum PropertyID : uint {
        Players = 1,
    }
    
    #region Properties
    
    private ModelProperty<Normal.Realtime.Serialization.RealtimeDictionary<VoiceChatModel>> _playersProperty;
    
    #endregion
    
    public VoiceChatDictionaryModel() : base(null) {
        RealtimeModel[] childModels = new RealtimeModel[1];
        
        _players = new Normal.Realtime.Serialization.RealtimeDictionary<VoiceChatModel>();
        childModels[0] = _players;
        
        SetChildren(childModels);
        
        _playersProperty = new ModelProperty<Normal.Realtime.Serialization.RealtimeDictionary<VoiceChatModel>>(1, _players);
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _playersProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _playersProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.Players: {
                    changed = _playersProperty.Read(stream, context);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _players = players;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
