using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeOnEnable : MonoBehaviour
{
    private CanvasGroup m_group;

    void Awake()
    {
        m_group = this.GetComponent<CanvasGroup>();
    }
    void OnEnable()
    {
        m_group.alpha = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        m_group.alpha = Mathf.Lerp(m_group.alpha, 1.0f, Time.deltaTime*5.0f);
    }
}
