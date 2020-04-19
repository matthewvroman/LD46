using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager s_instance;
    public static LevelManager Instance 
    {
        get
        {
            if(s_instance == null)
            {
                GameObject gameObject = new GameObject("LevelManager");
                s_instance = gameObject.AddComponent<LevelManager>();
                GameObject.DontDestroyOnLoad(gameObject);
            }
            return s_instance;
        }
    }
    private int m_level = 1;
    private int m_trueLevel = 1;
    public int TrueLevel { get => m_trueLevel; }
    public int Level { get => m_level; }

    public int[] m_experience = new int[]
    {
        100,
        275,
        650,
        1000,
        1000
    };

    private float[] m_totalHealth = new float[]
    {
        12,
        16,
        20,
        24,
        28
    };

    public float MaxPlayerHealth { get => m_totalHealth[Level-1]; }

    private int m_currentExperience = 0;
    public int CurrentExperience { get => m_currentExperience; }

    public int TotalExperience { get => m_experience[Level-1]; }

    private bool m_gameComplete;
    public bool GameComplete { get => m_gameComplete; set => m_gameComplete = value; }

    public Action<int> OnLevelUp;

    private void OnEnable()
    {
        Enemy.Killed += OnEnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.Killed -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        m_currentExperience+=enemy.Experience;
        if(m_currentExperience >= TotalExperience)
        {
            //LEVEL UP!!!!
            m_currentExperience -= TotalExperience;
            m_level++;
            m_trueLevel++;
            m_level = Mathf.Min(m_level, m_experience.Length);
            if(OnLevelUp != null) OnLevelUp(Level);
        }
    }
}
