using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOscillator : MonoBehaviour
{
    [SerializeField] private Vector3 m_rotation;
    [SerializeField] private Vector3 m_offset;
    [SerializeField] private float m_rate;


    // Update is called once per frame
    void Update()
    {
        Vector3 oscillatedRotation = m_rotation * Mathf.Sin(2 * Mathf.PI * Time.realtimeSinceStartup * m_rate);
        this.transform.localEulerAngles = m_offset + oscillatedRotation;
    }
}
