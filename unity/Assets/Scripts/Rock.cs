using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : SpellAssist
{
    [SerializeField] private GameObject m_prefab;
    private float m_timeUntilNextRock;
    private float m_timeBetweenRocks = 0.15f;
    private float m_spaceBetweenRocks = 1.25f;
    
    private PlayerController m_player;
    private Vector3 m_startPosition;
    private Vector3 m_endPosition;
    private Vector3 normalized;
    private int numRocks;
    private int rockNum;
    private CameraEffects m_cameraEffects;
    private Vector3 offset = new Vector3(0, 0.45f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        m_timeUntilNextRock = m_timeBetweenRocks;
    }

    public override void SetSpell(Spell spell, Enemy target)
    {
        base.SetSpell(spell, target);

        m_player = GameObject.FindObjectOfType<PlayerController>();
        m_cameraEffects = GameObject.FindObjectOfType<CameraEffects>();
        m_startPosition = m_player.transform.position;
        m_endPosition = this.transform.position;
        numRocks = (int)Mathf.Ceil((m_endPosition - m_startPosition).magnitude / m_spaceBetweenRocks);
        rockNum = 0;
        normalized = (m_endPosition - m_startPosition).normalized;
        m_endPosition = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_timeUntilNextRock -= Time.deltaTime;
        if(m_timeUntilNextRock <= 0.0f)
        {
            //spawn rock
            rockNum++;
            Vector3 position = Vector3.Lerp(m_startPosition, m_endPosition, (float)rockNum/(float)numRocks);
            position += offset;
            //stagger
            if(rockNum%2==0)
            {
                position.y += 0.15f;
            }
            else
            {
                position.y -= 0.15f;
            }
            GameObject rockInstance = GameObject.Instantiate(m_prefab);
            rockInstance.transform.position = position;
            m_cameraEffects.Shake(new Vector3(0.1f, 0.1f), 0.15f);
            m_timeUntilNextRock = m_timeBetweenRocks;

            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            foreach(Enemy enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, position);
                if(distance > 2.0f) continue;
                enemy.Damage(m_spell.Damage, m_spell.Impulse, 0.25f);
            }

            if(rockNum==numRocks)
            {
                GameObject.Destroy(this.gameObject);
            }
            
            //GameObject.Destroy(this.gameObject);
        }
    }
}
