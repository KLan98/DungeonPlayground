using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager
{
    // local states, should they be removed?
    //private List<TestEntity> entities; // list of entities in each room will vary, player always included
    private CurrentEntity currentEntity;

    public TurnManager()
    {
        //entities = new List<TestEntity>();
        //TestEntity player = new TestEntity() { id = 0, speed = 10 };
        //TestEntity enemy1 = new TestEntity() { id = 1, speed = 10 };
        //TestEntity enemy2 = new TestEntity() { id = 2, speed = 20 };
        //TestEntity enemy3 = new TestEntity() { id = 3, speed = 5 };
        //TestEntity enemy4 = new TestEntity() { id = 4, speed = 100 };

        //entities.Add(player);
        //entities.Add(enemy1);
        //entities.Add(enemy2);
        //entities.Add(enemy3);
        //entities.Add(enemy4);
    }

    public void StartCombat()
    {
        // get entities list from somewhere...

        // for testing purpose the entities list has already been constructed

        // construct the turntable by sorting id from entities list based on their speed
        // implement quicksort
        //MyAPI.QuickSort(entities);

        UpdateTurn();
    }

    public void EndCombat()
    {
        //entities.Clear();
    }

    // called whenever an entity finishes its action 
    public void UpdateTurn()
    {
        // assign the current actor to be something else
        //currentEntity.Index++;
        
        //if (currentEntity.Index == entities.Count)
        //{
        //    currentEntity.Index = 0;
        //    EndCombat();
        //    return;
        //}

        //currentEntity = entities[currentEntity.Index];
        
        //// if player's turn
        //if (currentEntity.ID == 0)
        //{
        //    StartPlayerTurn();
        //}

        //else
        //{
        //    StartEnemyTurn();
        //}
    }

    public void StartPlayerTurn()
    {
        // Player can act

        // update turn
        UpdateTurn();
    }

    public void StartEnemyTurn()
    {
        // Enemy can act

        UpdateTurn();
    }
}

public struct CurrentEntity
{
    public int ID;
    public int Index;
}