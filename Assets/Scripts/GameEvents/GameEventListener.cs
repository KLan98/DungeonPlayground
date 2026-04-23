using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;
    [SerializeField] private List<EventAndResponsePair> eventAndResponsePairs = new List<EventAndResponsePair>();

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);

        foreach(var pair in eventAndResponsePairs)
        {
            pair.gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);

        foreach(var pair in eventAndResponsePairs)
        {
            pair.gameEvent.UnregisterListener(this);
        }
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }

    public void OnGameEventRaised(GameEvent gameEvent)
    {
        UnityEvent response = GetResponseForGameEvent(gameEvent);

        if (response != null)
        {
            response.Invoke();
            Debug.Log($"game event {gameEvent} raised");
        }
    }

    private UnityEvent GetResponseForGameEvent(GameEvent gameEvent)
    {
        foreach (var pair in eventAndResponsePairs)
        {
            if (pair.gameEvent == gameEvent)
            {
                return pair.response;
            }
        }

        return null;
    }
}

[System.Serializable]
public struct EventAndResponsePair
{
    public GameEvent gameEvent;
    public UnityEvent response;
}
