using System;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerInputID
{
    //PHASE Game 0 -> 99
    LocalPlayerMove = 0,
    LocalPlayerRotate,
    LocalPlayerShoot,
}

[CreateAssetMenu(fileName = "PlayerInputEvent", menuName = "GameData/Event/PlayerInput", order = 1)]
public class PlayerInputEvent : EventBus
{
    private readonly Dictionary<int, Delegate> listeners = new();

    public override void AddListener<T>(int eventID, ActionCallback<T> callback)
    {
        // check if listener exist in distionary
        if (listeners.ContainsKey(eventID))
        {
            listeners[eventID] = Delegate.Combine(listeners[eventID], callback);
        }
        else
        {
            listeners.Add(eventID, callback);
        }
    }

    public override void ClearAllListener()
    {
        listeners.Clear();
    }

    public override void PostEvent<T>(int eventID, T param = default)
    {
        if (listeners.TryGetValue(eventID, out Delegate existingCallback))
        {
            (existingCallback as ActionCallback<T>)?.Invoke(param);
        }
    }

    public override void RemoveAllListener(int eventID)
    {
        if (listeners.ContainsKey(eventID))
            listeners[eventID] = null;
    }

    public override void RemoveListener<T>(int eventID, ActionCallback<T> callback)
    {
        if (listeners.ContainsKey(eventID))
        {
            listeners[eventID] = Delegate.Remove(listeners[eventID], callback);
            if (listeners[eventID] == null)
            {
                listeners.Remove(eventID);
            }
        }
    }
}
