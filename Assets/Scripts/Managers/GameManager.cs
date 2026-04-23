using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DungeonGrid grid;
    public Bomb_New Bomb;

    // Start is called before the first frame update
    void Start()
    {
        Bomb = new Bomb_New(grid);
        Debug.Log($"Instance of {Bomb.SkillID} with damage {Bomb.Damage.Value}, cost {Bomb.Cost.Value} created");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
