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

    [SerializeField] private GameObject m_portal;

    private void Awake()
    {
        s_instance = this;

        m_portal.SetActive(false);
    }

    public void SpawnPortal()
    {
        StartCoroutine(SpawnPortalCoroutine());
    }

    private IEnumerator SpawnPortalCoroutine()
    {
        yield return new WaitForSeconds(0.25f);

        m_portal.SetActive(true);
    }
}
