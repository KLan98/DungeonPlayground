using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [Header("Debug")]
    [SerializeField] private EntitiesDatabase entitiesDatabase;
    [SerializeField] private SkillsDatabase skillsDatabase;
    [SerializeField] private PlayerSkills playerSkills;

    private void Awake()
    {
        entitiesDatabase = new EntitiesDatabase();

        skillsDatabase = new SkillsDatabase(); 
        playerSkills = new PlayerSkills();

        instance = this;
    }

    private void Start()
    {
    }

    public static GameManager GetInstance()
    {
        return instance;
    }

    public SkillsDatabase GetSkillsDatabase()
    {
        return skillsDatabase;
    }

    public EntitiesDatabase GetEntitiesDatabase()
    {
        return entitiesDatabase;
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }
}
