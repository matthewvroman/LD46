using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashOnHit : MonoBehaviour
{
    private SpriteRenderer m_renderer;
    private float m_duration;
    private float m_speed = 5.0f;

    public bool Flashing { get => m_duration>0; }

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
            float value = Mathf.Sin(2 * Mathf.PI * Time.realtimeSinceStartup * m_speed) * 0.5f + 0.5f;
            Color color = m_renderer.color;
            color.a = value;
            m_renderer.color = color;
            if(m_duration<=0)
            {
                m_renderer.color = Color.white;
            }
        }
    }
}
