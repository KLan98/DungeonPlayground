using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericChannelListener<T> : MonoBehaviour
{
    // a listener can listen to many event channels, giving it the capability to response to different events
    [SerializeField] private List<ChannelEventPairs<T>> channelEventPairs = new List<ChannelEventPairs<T>>();

    protected void OnEnable()
    {
        foreach (var element in channelEventPairs)
        {
            // register this event with channel
            element.Channel.AddListener(this);
        }
    }

    protected void OnDisable()
    {
        foreach (var element in channelEventPairs)
        {
            // deregister this event with channel
            element.Channel.RemoveListener(this);
        }
    }

    public void InvokeResponse(GenericEventChannel<T> channel, T data)
    {
        foreach (var element in channelEventPairs)
        {
            // if the channel for this element = channel raising the event 
            if (element.Channel == channel)
            {
                // then invoke event with data
                element.Event.Invoke(data);
            }
        }
    }
}

[System.Serializable]
public struct ChannelEventPairs<T>
{
    public GenericEventChannel<T> Channel;
    public UnityEvent<T> Event;
}