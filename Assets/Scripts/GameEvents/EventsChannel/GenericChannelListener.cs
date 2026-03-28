using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericChannelListener<T> : MonoBehaviour
{
    [SerializeField] private List<ChannelEventPairs<T>> channelEventPairs;

    private void OnEnable()
    {
        foreach (var element in channelEventPairs)
        {
            element.Channel.AddListener(this);
        }
    }

    private void OnDisable()
    {
        foreach (var element in channelEventPairs)
        {
            element.Channel.RemoveListener(this);
        }
    }

    public void InvokeResponse(T data)
    {
        foreach (var element in channelEventPairs)
        {
            element.Event.Invoke(data);
        }
    }
}

[System.Serializable]
public struct ChannelEventPairs<T>
{
    public EventChannel<T> Channel;
    public UnityEvent<T> Event;
}