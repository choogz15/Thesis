using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class CallSyncModel
{
    [RealtimeProperty(1, true, true)] private int _dialingPlayer;
    [RealtimeProperty(2, true, true)] private int _dialerPlayer;
    [RealtimeProperty(4, true, true)] private int _talkingPlayer;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class CallSyncModel : RealtimeModel {
    public int dialingPlayer {
        get {
            return _dialingPlayerProperty.value;
        }
        set {
            if (_dialingPlayerProperty.value == value) return;
            _dialingPlayerProperty.value = value;
            InvalidateReliableLength();
            FireDialingPlayerDidChange(value);
        }
    }
    
    public int dialerPlayer {
        get {
            return _dialerPlayerProperty.value;
        }
        set {
            if (_dialerPlayerProperty.value == value) return;
            _dialerPlayerProperty.value = value;
            InvalidateReliableLength();
            FireDialerPlayerDidChange(value);
        }
    }
    
    public int talkingPlayer {
        get {
            return _talkingPlayerProperty.value;
        }
        set {
            if (_talkingPlayerProperty.value == value) return;
            _talkingPlayerProperty.value = value;
            InvalidateReliableLength();
            FireTalkingPlayerDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(CallSyncModel model, T value);
    public event PropertyChangedHandler<int> dialingPlayerDidChange;
    public event PropertyChangedHandler<int> dialerPlayerDidChange;
    public event PropertyChangedHandler<int> talkingPlayerDidChange;
    
    public enum PropertyID : uint {
        DialingPlayer = 1,
        DialerPlayer = 2,
        TalkingPlayer = 4,
    }
    
    #region Properties
    
    private ReliableProperty<int> _dialingPlayerProperty;
    
    private ReliableProperty<int> _dialerPlayerProperty;
    
    private ReliableProperty<int> _talkingPlayerProperty;
    
    #endregion
    
    public CallSyncModel() : base(null) {
        _dialingPlayerProperty = new ReliableProperty<int>(1, _dialingPlayer);
        _dialerPlayerProperty = new ReliableProperty<int>(2, _dialerPlayer);
        _talkingPlayerProperty = new ReliableProperty<int>(4, _talkingPlayer);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _dialingPlayerProperty.UnsubscribeCallback();
        _dialerPlayerProperty.UnsubscribeCallback();
        _talkingPlayerProperty.UnsubscribeCallback();
    }
    
    private void FireDialingPlayerDidChange(int value) {
        try {
            dialingPlayerDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireDialerPlayerDidChange(int value) {
        try {
            dialerPlayerDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireTalkingPlayerDidChange(int value) {
        try {
            talkingPlayerDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _dialingPlayerProperty.WriteLength(context);
        length += _dialerPlayerProperty.WriteLength(context);
        length += _talkingPlayerProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _dialingPlayerProperty.Write(stream, context);
        writes |= _dialerPlayerProperty.Write(stream, context);
        writes |= _talkingPlayerProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.DialingPlayer: {
                    changed = _dialingPlayerProperty.Read(stream, context);
                    if (changed) FireDialingPlayerDidChange(dialingPlayer);
                    break;
                }
                case (uint) PropertyID.DialerPlayer: {
                    changed = _dialerPlayerProperty.Read(stream, context);
                    if (changed) FireDialerPlayerDidChange(dialerPlayer);
                    break;
                }
                case (uint) PropertyID.TalkingPlayer: {
                    changed = _talkingPlayerProperty.Read(stream, context);
                    if (changed) FireTalkingPlayerDidChange(talkingPlayer);
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
        _dialingPlayer = dialingPlayer;
        _dialerPlayer = dialerPlayer;
        _talkingPlayer = talkingPlayer;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
