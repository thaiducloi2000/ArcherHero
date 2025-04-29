using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEvenListener
{
    public void SetupEventListener();
    public void RemoveEventListener();
}

public enum GameplayEventID
{
    //PHASE Game 0 -> 99
    StateChange = 0,
    OnStateChangeDone = 10,

    //Player Event 100 -> 199
    OnPlayerGetDamage = 100,
    OnPlayerMoveInput,
    OnPlayerRotateInput,
    
    // Chess Event 200 -> 299
    OnChessGetDamage = 200,
    OnChessSpawnAt,
    OnSelectChess,
    OnSelectChessClick,
    OnCheckChessContainsOnBoard,
    OnMoveChessOnBoard,
    OnGetChessInBoard,
    OnChessesPositionChange,
    OnChessSpawn,

    // Enemy Event
    OnEnemySpawn = 300,
    OnEnemyDamaged,
    OnEnemyDeath,
    EnemyRelease,
    OnCurrentEnemyTargetDeath,
}

[CreateAssetMenu(fileName = "GameplayEvent", menuName = "GameData/Event/Gameplay", order = 0)]
public class GameplayEvent : EventBus
{
    private readonly Dictionary<int, Delegate> listeners = new();

    public override void AddListener<T>(int eventID, ActionCallback<T> callback)
    {
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
