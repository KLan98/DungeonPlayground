using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "SO/NewGameEvent")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> eventListeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        eventListeners.Remove(listener);
    }

    public void RaiseGameEvent(GameEvent gameEvent)
    {
        foreach (GameEventListener listener in eventListeners)
        {
            listener.OnGameEventRaised(gameEvent);
        }
    }
}
