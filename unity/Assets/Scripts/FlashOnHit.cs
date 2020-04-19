using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashOnHit : MonoBehaviour
{
    private SpriteRenderer m_renderer;
    private float m_duration;

    private void Awake()
    {
        m_renderer = this.GetComponent<SpriteRenderer>();
    }

    public void Flash(float duration)
    {
        m_duration = duration;
    }

    private void Update()
    {
        if(m_duration > 0)
        {
            m_duration -= Time.deltaTime;

            //Mathf.Sin(Time.realtimeSinceStartup) * 0.5f + 0.5f;
            if(m_duration<=0)
            {
                m_renderer.color = Color.white;
            }
        }
    }
}
