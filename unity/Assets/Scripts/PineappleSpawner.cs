using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PineappleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_pineapplePrefab;
    [SerializeField] private RectTransform m_spawnRect;
    private float m_timeBetweenPineapples = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_timeBetweenPineapples>0.0f)
        {
            m_timeBetweenPineapples -= Time.deltaTime;
            if(m_timeBetweenPineapples<=0.0f)
            {
                Spawn();
                m_timeBetweenPineapples = 0.15f;
            }
        }
    }

    void Spawn()
    {
        GameObject gameObject = GameObject.Instantiate(m_pineapplePrefab, this.transform);
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(UnityEngine.Random.Range(m_spawnRect.rect.xMin, m_spawnRect.rect.xMax), UnityEngine.Random.Range(m_spawnRect.rect.yMin, m_spawnRect.rect.yMax));
        
        float fallSpeed = UnityEngine.Random.Range(-350, -180);
        float rotateSpeed = UnityEngine.Random.Range(35,60);
        float scale = UnityEngine.Random.Range(0.15f, 0.4f);
        gameObject.GetComponent<Mover>().Speed = new Vector3(0, fallSpeed, 0);
        gameObject.GetComponent<Rotate>().Speed = new Vector3(0, 0, rotateSpeed);
        gameObject.transform.localScale = Vector3.one * scale;

    }
}
