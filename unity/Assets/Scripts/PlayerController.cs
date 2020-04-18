using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_desiredDirection;

    // Start is called before the first frame update
    void Start()
    {
        m_desiredDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = m_animator.gameObject.transform.localScale;
        scale.x = Mathf.Lerp(scale.x, m_desiredDirection, Time.deltaTime * 15.0f);
        m_animator.gameObject.transform.localScale = scale;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        //direction
        m_animator.SetBool("Running", horizontal != 0);
        if(horizontal != 0)
        {
            m_desiredDirection = horizontal<0?-1:1;
        }

        m_rigidbody.velocity = Vector2.right * horizontal * m_speed;
    }
}
