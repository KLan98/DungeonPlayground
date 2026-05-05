using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntitiesDatabase 
{
    [SerializeField] private List<Client> aliveEntities;
    [SerializeField] private List<Client> deadEntities;
    [SerializeField] private List<Client> targettedEntities;

    public EntitiesDatabase() 
    {
        aliveEntities = new List<Client>();
        deadEntities = new List<Client>();
        targettedEntities = new List<Client>();
    }

    //-------------------------------ALIVE ENTITIES---------------------------
    public List<Client> GetAliveEntities()
    {
        return aliveEntities;
    }

    public void AddAliveEntity(Client entity)
    {
        aliveEntities.Add(entity);
    }

    public void RemoveAliveEntity(Client entity)
    {
        aliveEntities.Remove(entity);
    }

    public void ClearAliveEntity()
    {
        aliveEntities.Clear();
    }

    public int CountAliveEntities()
    {
        return aliveEntities.Count;
    }

    //--------------------------TARGETTED ENTITIES-----------------------------
    public List<Client> GetTargettedEntities()
    {
        return targettedEntities;
    }

    public void AddTargettedEntity(Client entity)
    {
        targettedEntities.Add(entity);
    }

    public void RemoveTargettedEntity(Client entity)
    {
        targettedEntities.Remove(entity);
    }

    public void ClearTargettedEntities()
    {
        targettedEntities.Clear();
    }

    public int CountTargettedEntities()
    {
        return targettedEntities.Count;
    }
}
