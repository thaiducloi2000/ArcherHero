/// <summary>
/// Interface to using Event Bus system
/// </summary>
public interface IBusdata<T> { }
public delegate void ActionCallback<T>(T param);
public interface IEventBus
{ /// <summary>
  /// Register to listen for eventID
  /// </summary>
  /// <param name="eventID">EventID that object want to listen</param>
  /// <param name="callback">Callback will be invoked when this eventID be raised</param>
    public void AddListener<T>(int eventID, ActionCallback<T> callback);

    /// <summary>
    /// Posts the event with param. This will notify all listener that register for this event
    /// </summary>
    /// <param name="eventID">EventID.</param>
    /// <param name="param">Parameter. Can be anything (struct, class ...), Data must be implement IEvenData to register</param>
    public void PostEvent<T>(int eventID, T param = default);

    /// <summary>
    /// Removes the listener has param. Use to Unregister listener
    /// </summary>
    /// <param name="eventID">EventID.</param>
    /// <param name="callback">Callback.</param>
    public void RemoveListener<T>(int eventID, ActionCallback<T> callback);

    /// <summary>
    /// Removes the listener has param. Use to Unregister listener
    /// </summary>
    /// <param name="eventID">EventID.</param>
    /// <param name="callback">Callback.</param>
    public void RemoveAllListener(int eventID);

    /// <summary>
    /// Clears all the listener.
    /// </summary>
    public void ClearAllListener();
}
