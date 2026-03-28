using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChannel<T> : ScriptableObject
{
    protected List<GenericChannelListener<T>> listOfListeners;

    public void AddListener(GenericChannelListener<T> listener)
    {
        listOfListeners.Add(listener);
    }

    public void RemoveListener(GenericChannelListener<T> listener) 
    { 
        listOfListeners.Remove(listener);
    }

    public void RaiseEvent(T data)
    {
        foreach (var element in listOfListeners)
        {
            if (element != null)
            {
                element.InvokeResponse(data);
            }
        }
    }
}
