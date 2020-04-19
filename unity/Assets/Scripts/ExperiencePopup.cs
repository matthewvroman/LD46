using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePopup : MonoBehaviour
{
    [SerializeField] Text m_text;
    [SerializeField] private CanvasGroup m_canvasGroup;
    private RectTransform m_rectTransform;

    private float m_timeElapsed = 0.0f;

    private void Awake()
    {
        m_rectTransform = this.GetComponent<RectTransform>();
    }
    public void Show(Vector3 worldPosition, int experience)
    {
        m_text.text = experience.ToString("n0");
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
        Canvas canvas = this.GetComponentInParent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 anchoredPosition = new Vector2( (viewportPosition.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f),
                                                ((viewportPosition.y*canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)));
        this.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }

    private void Update()
    {
        m_timeElapsed += Time.deltaTime;

        this.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Easing.EaseOutBack(Mathf.Min(1.0f,m_timeElapsed/0.35f)));

        Vector2 pos = m_rectTransform.anchoredPosition;
        pos.y += Time.deltaTime * 250.0f;
        m_rectTransform.anchoredPosition = pos;

        if(m_timeElapsed>0.5f)
        {
            m_canvasGroup.alpha = Mathf.Lerp(1, 0, Mathf.Max(0.0f, (m_timeElapsed-0.5f)/0.35f));
            if(m_canvasGroup.alpha < 0.1f)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
