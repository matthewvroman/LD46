using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    public SpriteRenderer Renderer { get => m_spriteRenderer; set => m_spriteRenderer = value; }
    [SerializeField] private float m_fadeTime;
    public float FadeTime { get => m_fadeTime; set => m_fadeTime = value; }
    private float m_elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        m_elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_elapsedTime += Time.deltaTime;
        m_elapsedTime = Mathf.Min(m_fadeTime, m_elapsedTime);
        Color color = m_spriteRenderer.color;
        color.a = Mathf.Lerp(1, 0, m_elapsedTime/m_fadeTime);
        m_spriteRenderer.color = color;
        if(m_elapsedTime==m_fadeTime)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
