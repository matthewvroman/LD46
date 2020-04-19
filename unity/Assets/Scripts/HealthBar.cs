using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Enemy m_enemy;
    [SerializeField] private Sprite[] m_heartSprites;
    private SpriteRenderer[] m_spriteRenderers;
    [SerializeField] private float m_heartOffsets;
    
    private void Start()
    {
        m_enemy.OnDamaged += OnDamaged;
    }

    private void OnDamaged()
    {
        float healthPerHeartPiece = 1.0f;
        if(m_spriteRenderers == null)
        {
            int numRenderers = (int)Mathf.Ceil(m_enemy.MaxHealth/(healthPerHeartPiece * m_heartSprites.Length));
            m_spriteRenderers = new SpriteRenderer[numRenderers];
            for(int i=0; i<numRenderers; i++)
            {
                GameObject gameObject = new GameObject("Heart");
                SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
                gameObject.transform.SetParent(this.transform);
                Vector3 position = new Vector3(m_heartOffsets*i - (m_heartOffsets*(numRenderers-1))/2.0f, 0, 0);
                gameObject.transform.localPosition = position;
                m_spriteRenderers[i] = renderer;
            }
            this.gameObject.transform.localPosition = m_enemy.HealthBarOffset;
        }

        int numFullHearts = (int)Mathf.Floor(m_enemy.CurrentHealth/(healthPerHeartPiece * m_heartSprites.Length));
        int pieceCount = (int)m_enemy.CurrentHealth%(int)(healthPerHeartPiece*m_heartSprites.Length);
        for(int i=0; i<m_spriteRenderers.Length; i++)
        {
            if(i<numFullHearts)
            {
                m_spriteRenderers[i].sprite = m_heartSprites[m_heartSprites.Length-1];
            }
            else if(i==numFullHearts && pieceCount>0)
            {
                m_spriteRenderers[i].sprite = m_heartSprites[pieceCount-1];
            }
            else
            {
                m_spriteRenderers[i].sprite = null;
            }
        }
    }
}
