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
    [SerializeField] private DialogueCharacter m_introDialogueCharacter;

    [SerializeField] private PlayerController m_player;
    [SerializeField] private Enemy[] m_enemyPrefabs;

    [SerializeField] private Portal m_expelPortal;

    [SerializeField] private GameObject m_hud;

    public Action OnSpawnEnemies;
    

    private void Awake()
    {
        if(s_instance != null)
        {
            s_instance = this;
        }

        Enemy.Killed += OnEnemyKilled;
        
        StartCoroutine(LevelIntro());
        //m_dialoguePanel.Display(m_introDialogueCharacter, m_introDialogues[LevelManager.Instance.Level-1]);
    }

    private void OnDisable()
    {
        Enemy.Killed -= OnEnemyKilled;

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
        m_dialoguePanel.Display(m_introDialogueCharacter, m_introDialogues[LevelManager.Instance.Level-1]);
    }

    private List<Enemy>m_aliveEnemies = new List<Enemy>();

    public void SpawnEnemies()
    {
        for(int i=0; i<5; i++)
        {
            Vector3 position = m_player.transform.position;
            position.x = UnityEngine.Random.Range(6.0f,9.0f);
            position.y = UnityEngine.Random.Range(-2.0f, -0.5f);
            Enemy enemy = GameObject.Instantiate(m_enemyPrefabs[0]);
            enemy.transform.position = position;
            m_aliveEnemies.Add(enemy);
        }
        if(OnSpawnEnemies != null) OnSpawnEnemies();
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        m_aliveEnemies.Remove(enemy);
        if(m_aliveEnemies.Count==0)
        {
            SpawnEnemies();
        }
    }
}
