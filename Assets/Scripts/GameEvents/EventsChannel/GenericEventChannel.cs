using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEventChannel<T> : ScriptableObject
{
    // An event channel can have many listeners, these are stored inside a list
    protected List<GenericChannelListener<T>> listOfListeners = new List<GenericChannelListener<T>>();

    // called in monobehavior 
    public void AddListener(GenericChannelListener<T> listener)
    {
        listOfListeners.Add(listener);
    }

    // called in monobehavior
    public void RemoveListener(GenericChannelListener<T> listener) 
    { 
        listOfListeners.Remove(listener);
    }

    public void RaiseEvent(T data)
    {
        foreach (var listener in listOfListeners)
        {
            if (listener != null)
            {
                // pass the data alongside with channel info to listener
                listener.InvokeResponse(this, data);
            }
        }
    }
}