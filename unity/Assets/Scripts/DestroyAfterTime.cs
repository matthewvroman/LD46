using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float m_time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_time -= Time.deltaTime;
        if(m_time<=0.0f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
