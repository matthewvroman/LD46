using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BattleTutorialText : MonoBehaviour
{
    [SerializeField] private Text m_text;
    private bool m_hide = false;
    
    void Start()
    {
        m_text = this.GetComponent<Text>();
        if(LevelManager.Instance.Level==1)
        {
            m_hide = true;
            m_text.text = "";
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if(LevelManager.Instance.Level==1)
        {
            BattleManager.Instance.OnSpawnEnemies += OnSpawnEnemies;
            Enemy.Killed += OnEnemyKilled;
        }  
    }

    private void OnDisable()
    {
        if(LevelManager.Instance.Level==1)
        {
           if(BattleManager.Instance) BattleManager.Instance.OnSpawnEnemies -= OnSpawnEnemies;
           Enemy.Killed -= OnEnemyKilled;
        }  
    }

    private void OnSpawnEnemies()
    {
        m_hide = false;
        m_text.text = "[Use Spacebar to Attack]";
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        m_hide = true;
    }

    private void Update()
    {
        Color color = m_text.color;
        float endValue = 1.0f;
        if(m_hide)
        {
            endValue = 0.0f;
        }
        color.a = Mathf.Lerp(color.a, endValue, Time.deltaTime*4.0f);
        m_text.color = color;
    }
}
