using UnityEngine;

public abstract class EventBus : ScriptableObject, IEventBus
{
    public abstract void AddListener<T>(int eventID, ActionCallback<T> callback);
    public abstract void ClearAllListener();
    public abstract void PostEvent<T>(int eventID, T param = default);
    public abstract void RemoveAllListener(int eventID);
    public abstract void RemoveListener<T>(int eventID, ActionCallback<T> callback);
}
