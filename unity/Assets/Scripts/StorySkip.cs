using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySkip : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private GameObject m_scene1;
    [SerializeField] private GameObject m_scene3;
    [SerializeField] private PineappleSpawner m_spawner;
    [SerializeField] private CanvasGroup m_titleCanvasGroup;
    [SerializeField] private GameObject m_title;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_animator.enabled = false;
            m_scene1.gameObject.SetActive(false);
            m_scene3.gameObject.SetActive(false);
            m_spawner.gameObject.SetActive(true);
            m_titleCanvasGroup.alpha = 1.0f;
            m_title.SetActive(true);
            this.gameObject.SetActive(false);
        }

        if(m_title.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }
}
