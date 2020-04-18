using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    private Vector3 m_cameraPosition;

    private float m_shakeDuration;
    private Vector2 m_shakeMagnitude;

    // Start is called before the first frame update
    void Start()
    {
        m_cameraPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_shakeDuration > 0)
        {
            m_shakeDuration -= Time.deltaTime;
            Vector3 position = m_cameraPosition;
            position.x += UnityEngine.Random.Range(-m_shakeMagnitude.x, m_shakeMagnitude.x);
            position.y += UnityEngine.Random.Range(-m_shakeMagnitude.y, m_shakeMagnitude.y);
            this.transform.position = position;
        }
        else
        {
            this.transform.position = m_cameraPosition;
        }
    }

    public void Shake(Vector2 magnitude, float duration)
    {
        m_shakeMagnitude = magnitude;
        m_shakeDuration = duration;
    }
}
