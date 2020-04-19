using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private PlayerController m_player;
    [SerializeField] private int m_spellIndex;
    [SerializeField] private GameObject m_cooldown;
    [SerializeField] private Text m_cooldownText;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Spell spell = m_player.Spells[m_spellIndex];

        if(!m_player.GameplayVersion || spell.MinLevel > LevelManager.Instance.Level)
        {
            this.gameObject.SetActive(false);
            return;
        }

        
        m_cooldown.SetActive(spell.CanCast==false);
        int cooldown = (int)Mathf.Max(Mathf.Ceil(spell.CurrentCooldown), 0);
        m_cooldownText.text = cooldown.ToString() + "s";
    }
}
