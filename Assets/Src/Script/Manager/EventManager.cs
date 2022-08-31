using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NoArgs {
}

public class SelectEventArgs {
    public Vector3 Mouse0StartPos;
    public Vector3 MouseCurrentPos;
}

public class TargetEventArgs {
    public Vector3 Mouse1StartPos;
    public Vector3 Mouse1DragDestPos;
}

public class BuildPrepareEventArgs {
    public BuildingData Data;
}

public class BuildFinishEventArgs {
    public bool IsConfirm;
    public bool IsDone;
}

public class UnitDestroyEventArgs {
    public Unit DestroyUnit;
}

public class EventManager : Singleton<EventManager> {
    private EventManager() {
    }


    private Dictionary<string, Action<object>> _events;

    void Awake() {
        _events = new();
    }

    public void AddListener(string eventName, Action<object> listener) {
        if (_events.TryGetValue(eventName, out var evt)) {
            evt += listener;
            _events[eventName] = evt;
        }
        else {
            _events.Add(eventName, listener);
        }
    }

    public void RemoveListener(string eventName, Action<object> listener) {
        if (_events.TryGetValue(eventName, out var evt)) {
            evt -= listener;
        }
    }

    public void OnEvent(string eventName, object arg) {
        if (_events.TryGetValue(eventName, out var evt)) {
            evt.Invoke(arg);
        }
    }
}