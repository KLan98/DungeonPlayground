using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAPI
{
    private const int tileSize = 1;
    /// <summary>
    /// Thinker contains modifiers here is where modifiers for skills are added. A skill need modifiers to have behaviors. Thinker is created OnSkillStart
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="origin"></param>
    /// <param name="thinkerParams"></param>
    public static void CreateThinker(PrimaryKey primaryKey, Vector2 origin)
    {
        GameObject thinker = new GameObject(primaryKey.SkillID.ToString() + " THINKER");
        thinker.transform.position = origin;

        switch (primaryKey.SkillID)
        {
            case SkillID.BOMB:
                thinker.AddComponent<ModifierBombExplode>().OnCreated(primaryKey);
                break;
        }
    }

    public static void ApplyDamage(List<byte> damageTable, byte amount)
    {
        List<Entity> aliveEntities = EntitiesManager.GetInstance().GetAliveEntities();
        for (int i = 0; i < damageTable.Count; i++)
        {
            for (int j = 0; j < aliveEntities.Count; j++)
            {
                if (damageTable[i] == aliveEntities[j].EntityID)
                {
                    Entity entity = aliveEntities[j];
               
                    entity.EntityStats.HitPoint -= amount;

                    aliveEntities[j] = entity;

                    Debug.Log($"Deal {amount} damage to {entity.GameObject.name} with EntityID = {entity.EntityID}, current HP = {entity.EntityStats.HitPoint}");
                }
            }
        }

        aliveEntities.RemoveAll(IsDead);
    }

    private static bool IsDead(Entity entity)
    {
        if (entity.EntityStats.HitPoint <= 0)
        {
            entity.GameObject.SetActive(false);
            EntitiesManager.GetInstance().AddDeadEntity(entity);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Take in world position and convert to cell
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public static Vector2 GetCellCenter(Vector2 worldPosition)
    {
        int xIndex = Mathf.FloorToInt(worldPosition.x / tileSize);
        int yIndex = Mathf.FloorToInt(worldPosition.y / tileSize);
        return new Vector2(
            xIndex * tileSize + tileSize / 2f,
            yIndex * tileSize + tileSize / 2f
        );
    }

    //public static void QuickSort(List<TestEntity> entities)
    //{
    //    if (entities == null || entities.Count <= 1)
    //        return;

    //    QuickSortRecursive(entities, 0, entities.Count - 1);
    //}

    //-----------------------PRIVATE METHODS-----------------------------------------
    //private static void QuickSortRecursive(List<TestEntity> arr, int left, int right)
    //{
    //    // Small partition optimization
    //    while (left < right)
    //    {
    //        int pivotIndex = Partition(arr, left, right);

    //        // Recurse into smaller side first
    //        // reduces max recursion depth
    //        if (pivotIndex - left < right - pivotIndex)
    //        {
    //            QuickSortRecursive(arr, left, pivotIndex - 1);
    //            left = pivotIndex + 1;
    //        }
    //        else
    //        {
    //            QuickSortRecursive(arr, pivotIndex + 1, right);
    //            right = pivotIndex - 1;
    //        }
    //    }
    //}

    //private static int Partition(List<TestEntity> arr, int left, int right)
    //{
    //    // Median-of-three pivot selection
    //    int mid = left + (right - left) / 2;

    //    if (arr[left].speed < arr[mid].speed)
    //        Swap(arr, left, mid);

    //    if (arr[left].speed < arr[right].speed)
    //        Swap(arr, left, right);

    //    if (arr[mid].speed < arr[right].speed)
    //        Swap(arr, mid, right);

    //    // Use median as pivot
    //    TestEntity pivot = arr[mid];
    //    Swap(arr, mid, right);

    //    int storeIndex = left;

    //    // Greater-to-lower ordering
    //    for (int i = left; i < right; i++)
    //    {
    //        if (arr[i].speed > pivot.speed)
    //        {
    //            Swap(arr, i, storeIndex);
    //            storeIndex++;
    //        }
    //    }

    //    Swap(arr, storeIndex, right);

    //    return storeIndex;
    //}

    //private static void Swap(List<TestEntity> arr, int a, int b)
    //{
    //    if (a == b)
    //        return;

    //    TestEntity temp = arr[a];
    //    arr[a] = arr[b];
    //    arr[b] = temp;
    //}
}

public struct DamageTable
{
    public GameObject[] Victims;
    public Vector2Int Direction;
    public Vector2 Position;
    public byte Damage;
}
