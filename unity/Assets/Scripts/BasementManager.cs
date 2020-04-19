using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementManager : MonoBehaviour
{
    private static BasementManager s_instance;
    public static BasementManager Instance
    {
        get
        {
            return s_instance;
        }
    }

    private static bool s_seenIntro = false;

    [SerializeField] private Portal m_portal;
    [SerializeField] private GameObject m_intro;

    private void Awake()
    {
        s_instance = this;

        m_portal.gameObject.SetActive(false);
        if(LevelManager.Instance.TrueLevel>5) //just us and gorpol
        {
            m_portal.gameObject.SetActive(true);
        }

        if(s_seenIntro == false)
        {
            m_intro.SetActive(true);
            s_seenIntro = true;
        }
        else
        {
            m_intro.SetActive(false);
        }
    }

    public void SpawnPortal()
    {
        StartCoroutine(SpawnPortalCoroutine());
    }

    private IEnumerator SpawnPortalCoroutine()
    {
        yield return new WaitForSeconds(0.25f);

        m_portal.gameObject.SetActive(true);
        m_portal.Spawn();
    }
}
