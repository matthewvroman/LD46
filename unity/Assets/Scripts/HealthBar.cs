using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private IHealth m_healthInterface;
    [SerializeField] private Sprite[] m_heartSprites;
    private SpriteRenderer[] m_spriteRenderers;
    private Image[] m_images;
    [SerializeField] private float m_heartOffsets;
    [SerializeField] private Sortable m_sortable;

    private bool m_centered;
    private bool m_showUndamaged;
    private bool m_useSprites;
    
    public void SetInterface(IHealth healthInterface, bool showUndamaged=false, bool centered=true, bool useSprites=true)
    {
        m_healthInterface = healthInterface;
        m_healthInterface.OnDamaged += OnDamaged;
        m_centered = centered;
        m_showUndamaged = showUndamaged;
        m_useSprites = useSprites;

        if(m_showUndamaged)
        {
            OnDamaged();
        }
    }

    public void Reset()
    {
        if(m_spriteRenderers != null)
        {
            int numRenderers = m_spriteRenderers.Length;
            for(int i=0; i<numRenderers; i++)
            {
                if(m_spriteRenderers[i] != null) GameObject.Destroy(m_spriteRenderers[i].gameObject);
                if(m_images[i] != null) GameObject.Destroy(m_images[i].gameObject);
            }
            m_spriteRenderers = null;
            m_images = null;
            SetInterface(m_healthInterface, m_showUndamaged, m_centered, m_useSprites);
        }
    }

    private void OnDamaged()
    {
        Debug.Log("OnDamaged::" + this.gameObject.name);
        float healthPerHeartPiece = 1.0f;
        if(m_spriteRenderers == null)
        {
            int numRenderers = (int)Mathf.Ceil(m_healthInterface.MaxHealth/(healthPerHeartPiece * m_heartSprites.Length));
            m_spriteRenderers = new SpriteRenderer[numRenderers];
            m_images = new Image[numRenderers];
            for(int i=0; i<numRenderers; i++)
            {
                GameObject gameObject = new GameObject("Heart");

                gameObject.transform.SetParent(this.transform);
                Vector3 position = new Vector3(m_heartOffsets*i, 0, 0);
                if(m_centered)
                {
                    position.x -= (m_heartOffsets*(numRenderers-1))/2.0f;
                }
                gameObject.transform.localPosition = position;
                if(m_useSprites)
                {
                    SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer>();
                    m_spriteRenderers[i] = renderer;
                }
                else
                {
                    Image image = gameObject.AddComponent<Image>();
                    m_images[i] = image;
                }
                
            }
            if(m_useSprites && m_sortable != null)
            {
                m_sortable.AddExtras(m_spriteRenderers);
            }
            this.gameObject.transform.localPosition = m_healthInterface.HealthBarOffset;
        }

        int numFullHearts = (int)Mathf.Floor(m_healthInterface.CurrentHealth/(healthPerHeartPiece * m_heartSprites.Length));
        int pieceCount = (int)m_healthInterface.CurrentHealth%(int)(healthPerHeartPiece*m_heartSprites.Length);
        for(int i=0; i<m_spriteRenderers.Length; i++)
        {
            if(i<numFullHearts)
            {
                if(m_useSprites)
                {
                    m_spriteRenderers[i].sprite = m_heartSprites[m_heartSprites.Length-1];
                } 
                else
                {
                    m_images[i].color = Color.white;
                    m_images[i].sprite = m_heartSprites[m_heartSprites.Length-1]; m_images[i].SetNativeSize();
                }
            }
            else if(i==numFullHearts && pieceCount>0)
            {
                if(m_useSprites)
                {
                    m_spriteRenderers[i].sprite = m_heartSprites[pieceCount-1];
                }
                else
                {
                    m_images[i].color = Color.white;
                    m_images[i].sprite = m_heartSprites[pieceCount-1]; m_images[i].SetNativeSize();
                }
            }
            else
            {
                if(m_useSprites)
                {
                    m_spriteRenderers[i].sprite = null;
                }
                else
                {
                    m_images[i].sprite = null; m_images[i].SetNativeSize();
                    m_images[i].color = Color.clear;
                }
            }
        }
    }
}
