using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Debug")]
    [SerializeField] private SkillsDatabase skillsDatabase;
    [SerializeField] private PlayerSkills playerSkills;

    private TurnManager turnManager;

    private void Awake()
    {
        //turnManager = new TurnManager();
        skillsDatabase = new SkillsDatabase();
        playerSkills = new PlayerSkills();

        if (instance != null && instance != this)
        {
            return;
        }

        instance = this;
    }

    private void Start()
    {
        //turnManager.StartCombat();
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public SkillsDatabase GetSkillsDatabase()
    {
        return skillsDatabase;
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }

    public TurnManager GetTurnManager()
    {
        return turnManager;
    }
}
