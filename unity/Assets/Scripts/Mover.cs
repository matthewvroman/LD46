using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 m_speed;
    public Vector3 Speed { get => m_speed;  set => m_speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = (m_speed * Time.deltaTime);
        this.transform.localPosition += movement;
    }
}
