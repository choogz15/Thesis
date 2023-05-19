using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class NPCModel
{
    [RealtimeProperty(1, true, true)] private string _animation;
    [RealtimeProperty(2, true, true)] private double _animationStartTime;
    [RealtimeProperty(3, true, true)] private int _stateNameHash;
    [RealtimeProperty(4, true, true)] private int _trigger;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class NPCModel : RealtimeModel {
    public string animation {
        get {
            return _animationProperty.value;
        }
        set {
            if (_animationProperty.value == value) return;
            _animationProperty.value = value;
            InvalidateReliableLength();
            FireAnimationDidChange(value);
        }
    }
    
    public double animationStartTime {
        get {
            return _animationStartTimeProperty.value;
        }
        set {
            if (_animationStartTimeProperty.value == value) return;
            _animationStartTimeProperty.value = value;
            InvalidateReliableLength();
            FireAnimationStartTimeDidChange(value);
        }
    }
    
    public int stateNameHash {
        get {
            return _stateNameHashProperty.value;
        }
        set {
            if (_stateNameHashProperty.value == value) return;
            _stateNameHashProperty.value = value;
            InvalidateReliableLength();
            FireStateNameHashDidChange(value);
        }
    }
    
    public int trigger {
        get {
            return _triggerProperty.value;
        }
        set {
            if (_triggerProperty.value == value) return;
            _triggerProperty.value = value;
            InvalidateReliableLength();
            FireTriggerDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(NPCModel model, T value);
    public event PropertyChangedHandler<string> animationDidChange;
    public event PropertyChangedHandler<double> animationStartTimeDidChange;
    public event PropertyChangedHandler<int> stateNameHashDidChange;
    public event PropertyChangedHandler<int> triggerDidChange;
    
    public enum PropertyID : uint {
        Animation = 1,
        AnimationStartTime = 2,
        StateNameHash = 3,
        Trigger = 4,
    }
    
    #region Properties
    
    private ReliableProperty<string> _animationProperty;
    
    private ReliableProperty<double> _animationStartTimeProperty;
    
    private ReliableProperty<int> _stateNameHashProperty;
    
    private ReliableProperty<int> _triggerProperty;
    
    #endregion
    
    public NPCModel() : base(null) {
        _animationProperty = new ReliableProperty<string>(1, _animation);
        _animationStartTimeProperty = new ReliableProperty<double>(2, _animationStartTime);
        _stateNameHashProperty = new ReliableProperty<int>(3, _stateNameHash);
        _triggerProperty = new ReliableProperty<int>(4, _trigger);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _animationProperty.UnsubscribeCallback();
        _animationStartTimeProperty.UnsubscribeCallback();
        _stateNameHashProperty.UnsubscribeCallback();
        _triggerProperty.UnsubscribeCallback();
    }
    
    private void FireAnimationDidChange(string value) {
        try {
            animationDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireAnimationStartTimeDidChange(double value) {
        try {
            animationStartTimeDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireStateNameHashDidChange(int value) {
        try {
            stateNameHashDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireTriggerDidChange(int value) {
        try {
            triggerDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _animationProperty.WriteLength(context);
        length += _animationStartTimeProperty.WriteLength(context);
        length += _stateNameHashProperty.WriteLength(context);
        length += _triggerProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _animationProperty.Write(stream, context);
        writes |= _animationStartTimeProperty.Write(stream, context);
        writes |= _stateNameHashProperty.Write(stream, context);
        writes |= _triggerProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.Animation: {
                    changed = _animationProperty.Read(stream, context);
                    if (changed) FireAnimationDidChange(animation);
                    break;
                }
                case (uint) PropertyID.AnimationStartTime: {
                    changed = _animationStartTimeProperty.Read(stream, context);
                    if (changed) FireAnimationStartTimeDidChange(animationStartTime);
                    break;
                }
                case (uint) PropertyID.StateNameHash: {
                    changed = _stateNameHashProperty.Read(stream, context);
                    if (changed) FireStateNameHashDidChange(stateNameHash);
                    break;
                }
                case (uint) PropertyID.Trigger: {
                    changed = _triggerProperty.Read(stream, context);
                    if (changed) FireTriggerDidChange(trigger);
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
        _animation = animation;
        _animationStartTime = animationStartTime;
        _stateNameHash = stateNameHash;
        _trigger = trigger;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
