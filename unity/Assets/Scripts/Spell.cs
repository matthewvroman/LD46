using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell")]
public class Spell : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private int m_maxTargets;
    [SerializeField] private float m_maxRange;
    [SerializeField] private bool m_frontFacingOnly;
    [SerializeField] private GameObject m_spellCircle;
    [SerializeField] private float m_spellCircleScale;
    [SerializeField] private float m_damage;
    [SerializeField] private GameObject m_damagePrefab;
    [SerializeField] private bool m_damagePrefabParentedToEnemy;
    [SerializeField] private Vector2 m_impulse;
    [SerializeField] private Vector2 m_screenShakeMagnitude;
    [SerializeField] private float m_screenShakeDuration = 0.1f;

    private List<Enemy> m_targets;
    private List<GameObject> m_spellCircles;

    public void MarkTargets(PlayerController player)
    {
        GetTargets(player);
    }

    public virtual void Cast(PlayerController player)
    {
        while(m_spellCircles.Count>0)
        {
            GameObject spellCircle = m_spellCircles[0];
            m_spellCircles.RemoveAt(0);
            SpriteRenderer renderer = spellCircle.GetComponentInChildren<SpriteRenderer>();
            FadeAndDestroy fd = spellCircle.AddComponent<FadeAndDestroy>();
            fd.Renderer = renderer;
            fd.FadeTime = 0.1f;
        }

        foreach(Enemy target in m_targets)
        {
            GameObject damageObject = GameObject.Instantiate(m_damagePrefab);
            if(m_damagePrefabParentedToEnemy)
            {
                damageObject.transform.SetParent(target.transform);
                damageObject.transform.localPosition = target.SpellCircleOffset;
            }
            target.Damage(m_damage, m_impulse);
        }

        if(m_screenShakeMagnitude.magnitude > 0 && m_targets.Count>0)
        {
            GameObject.FindObjectOfType<CameraEffects>().Shake(m_screenShakeMagnitude, m_screenShakeDuration);
        }
    }

    private void GetTargets(PlayerController player)
    {
        m_targets = new List<Enemy>();
        m_spellCircles = new List<GameObject>();
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemies)
        {
            if(enemy.Dead) continue;
            
            if(m_frontFacingOnly)
            {
                if(player.DesiredDirection < 0 && enemy.transform.position.x >= player.transform.position.x)
                {
                    continue;
                }
                if(player.DesiredDirection > 0 && enemy.transform.position.x <= player.transform.position.x)
                {
                    continue;
                }
            }
            player.StartCoroutine(AddSpellCircle(m_targets.Count*0.1f, enemy));
            m_targets.Add(enemy);
            
        }
    }

    private IEnumerator AddSpellCircle(float delay, Enemy enemy)
    {
        yield return new WaitForSeconds(delay);
        
        GameObject circle = GameObject.Instantiate(m_spellCircle, enemy.transform);
        circle.transform.localPosition = enemy.SpellCircleOffset;
        circle.transform.localScale = Vector3.one * m_spellCircleScale;
        m_spellCircles.Add(circle);
    }

    public void Update()
    {
        if(m_targets != null)
        {
            
        }
    }

}
