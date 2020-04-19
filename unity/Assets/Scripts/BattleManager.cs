using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager s_instance;
    public static BattleManager Instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = GameObject.FindObjectOfType<BattleManager>();
            }
            return s_instance;
        }
    }

    [SerializeField] private DialoguePanel m_dialoguePanel;

    //each index is for a different level
    [SerializeField] private List<Dialogue>m_introDialogues;
    [SerializeField] private DialogueCharacter[] m_introDialogueCharacter;

    [SerializeField] private List<Dialogue>m_exitDialogues;

    [SerializeField] private PlayerController m_player;
    [SerializeField] private Enemy[] m_enemyPrefabs;

    [SerializeField] private Portal m_expelPortal;
    [SerializeField] private Portal m_levelEndPortal;

    [SerializeField] private GameObject m_hud;

    [Serializable]
    private struct DifficultyAdjustment
    {
        public bool canSpawnBothSidesAtOnce;
        public int minEnemiesPerWave;
        public int maxEnemiesPerWave;
        public float damageModifier;
        public float speedModifier;
        public int expModifier;
        public float healthModifier;
    }

    [SerializeField] private DifficultyAdjustment[] m_difficulties;

    public Action OnSpawnEnemies;
    
    private bool m_levelled = false;
    private int m_initialLevel;

    private void Awake()
    {
        if(s_instance != null)
        {
            s_instance = this;
        }

        Enemy.Killed += OnEnemyKilled;
        LevelManager.Instance.OnLevelUp += OnLevelUp;

        m_initialLevel = LevelManager.Instance.Level;
        
        StartCoroutine(LevelIntro());
        //m_dialoguePanel.Display(m_introDialogueCharacter, m_introDialogues[LevelManager.Instance.Level-1]);
    }

    private void OnDisable()
    {
        Enemy.Killed -= OnEnemyKilled;
        LevelManager.Instance.OnLevelUp -= OnLevelUp;

        if(s_instance == this)
        {
            s_instance = null;
        }
    }

    public IEnumerator LevelIntro()
    {
        m_expelPortal.gameObject.SetActive(false);
        m_player.gameObject.SetActive(false);
        m_hud.gameObject.SetActive(false);

        Loading loading = GameObject.FindObjectOfType<Loading>();
        while(loading != null)
        {
            loading = GameObject.FindObjectOfType<Loading>();
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);
        m_expelPortal.gameObject.SetActive(true);
        m_expelPortal.Spawn();
        yield return new WaitForSeconds(0.35f);
        m_player.gameObject.SetActive(true);
        yield return StartCoroutine(m_expelPortal.Expel(m_player.gameObject));
        m_expelPortal.Despawn();
        m_hud.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_dialoguePanel.Display(m_introDialogueCharacter[LevelManager.Instance.Level-1], m_introDialogues[LevelManager.Instance.Level-1]);
    }

    public void SpawnPortal()
    {
        StartCoroutine(SpawnPortalCoroutine());
    }

    private IEnumerator SpawnPortalCoroutine()
    {
        yield return new WaitForSeconds(0.25f);

        m_levelEndPortal.gameObject.SetActive(true);
        m_levelEndPortal.Spawn();
    }

    private List<Enemy>m_aliveEnemies = new List<Enemy>();

    private int m_waveIndex = 0;
    
    public void SpawnEnemies()
    {
        DifficultyAdjustment difficulty = m_difficulties[m_initialLevel-1];
        int numEnemies = UnityEngine.Random.Range(difficulty.minEnemiesPerWave, difficulty.maxEnemiesPerWave);
        for(int i=0; i<numEnemies; i++)
        {
            Vector3 position = m_player.transform.position;
            if(m_waveIndex%2==0)
            {
                position.x = UnityEngine.Random.Range(6.0f,9.0f);
            }
            else
            {
                position.x = UnityEngine.Random.Range(-6.0f,-9.0f);
            }

            if(difficulty.canSpawnBothSidesAtOnce && UnityEngine.Random.Range(0.0f, 1.0f)>0.5f)
            {
                if(i>=numEnemies/2)
                {
                    position.x = -position.x;
                }
            }
            
            position.y = UnityEngine.Random.Range(-2.0f, -0.5f);
            Enemy enemy = GameObject.Instantiate(m_enemyPrefabs[0]);
            enemy.Augment(difficulty.healthModifier, difficulty.expModifier, difficulty.speedModifier, difficulty.damageModifier);
            enemy.transform.position = position;
            m_aliveEnemies.Add(enemy);
        }
        m_waveIndex++;
        if(OnSpawnEnemies != null) OnSpawnEnemies();
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        m_aliveEnemies.Remove(enemy);
        if(m_aliveEnemies.Count==0)
        {
            if(m_levelled)
            {
                if(LevelManager.Instance.TrueLevel > LevelManager.Instance.Level)
                {
                    m_dialoguePanel.Display(m_introDialogueCharacter[LevelManager.Instance.Level-1], m_exitDialogues[LevelManager.Instance.Level-1]);
                }
                else
                {
                    m_dialoguePanel.Display(m_introDialogueCharacter[LevelManager.Instance.Level-2], m_exitDialogues[LevelManager.Instance.Level-2]);
                }
            }
            else
            {
                SpawnEnemies();
            }
        }
    }

    private void OnLevelUp(int level)
    {
        m_levelled = true;
    }
}
