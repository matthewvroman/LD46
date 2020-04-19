using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private ExperiencePopup m_experiencePrefab;
    [SerializeField] private Text m_experienceDisplay;
    [SerializeField] private Text m_levelDisplay;

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
        ExperiencePopup popup = GameObject.Instantiate(m_experiencePrefab, this.transform);
        popup.Show(enemy.transform.position + enemy.HealthBarOffset, enemy.Experience);
    }

    private void Update()
    {
        m_levelDisplay.text = "Lv. " + LevelManager.Instance.TrueLevel.ToString("n0");
        m_experienceDisplay.text = "(" + LevelManager.Instance.CurrentExperience.ToString("n0") + "/" + LevelManager.Instance.TotalExperience.ToString("n0") + ")";
    }
}
