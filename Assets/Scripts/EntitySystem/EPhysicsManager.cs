using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EPhysicsManager : MonoBehaviour, IObserver, ISubject
{
    private static EPhysicsManager instance;
    private List<IObserver> observers;

    private void Awake()
    {
        if (instance != null && instance == this)
        {
            return;
        }

        instance = this;

        observers = new List<IObserver>();
    }

    public static EPhysicsManager GetInstance()
    {
        return instance;
    }

    //------------------------------------PUBLIC METHODS----------------------------------
    public void OnNotify()
    {
        Notify();
    }

    public void AddObserver(IObserver observer)
    {
        if(!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        if(observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    public void Notify()
    {
        foreach(var observer in observers)
        {
            observer.OnNotify();
        }
    }
}