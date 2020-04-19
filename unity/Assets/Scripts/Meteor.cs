using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor: SpellAssist
{
    private Vector3 m_startPosition;
    private Vector3 m_finalPosition;
    private float m_duration;
    private float m_timeElapsed;

    private void Start()
    {
        m_finalPosition = this.transform.position;
        m_startPosition = m_finalPosition;
        m_startPosition.y += 10.0f;
        m_duration = 0.25f;
        m_timeElapsed = 0.0f;
        this.transform.position = m_startPosition;
    }

    private void Update()
    {
        m_timeElapsed += Time.deltaTime;
        if(m_timeElapsed>m_duration)
        {
            m_timeElapsed = m_duration;
            m_target.Damage(m_spell.Damage, m_spell.Impulse);
            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                if(enemy == m_target) continue;
                Vector3 distance = enemy.transform.position - m_target.transform.position;
                if(distance.magnitude>2.0f) continue;
                if(enemy.EnemyState == Enemy.State.Dead) continue;

                enemy.Damage(m_spell.Damage, m_spell.Impulse);
            }
            if(m_target.Dead)
            {
                this.transform.parent.transform.SetParent(null);
            }
            GameObject.FindObjectOfType<CameraEffects>().Shake(new Vector3(0.25f, 0.25f), 0.2f);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            this.transform.position = Vector3.Lerp(m_startPosition, m_finalPosition, m_timeElapsed/m_duration);
        }
    }
}
