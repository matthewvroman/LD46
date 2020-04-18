using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Moving,
        Damaged,
        Dead
    }
    [SerializeField] private Rigidbody2D m_rigidbody;

    [SerializeField] private Vector3 m_spellCircleOffset;
    public Vector3 SpellCircleOffset { get => m_spellCircleOffset; }

    private State m_state;

    private PlayerController m_player;

    private float m_damageDuration;


    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_state == State.Damaged)
        {
            m_damageDuration -= Time.deltaTime;
            if(m_damageDuration <= 0)
            {
                m_state = State.Moving;
            }
        }
    }

    void FixedUpdate()
    {
        if(m_state == State.Moving)
        {
            m_rigidbody.velocity = Vector2.zero;
        }
    }

    public void Damage(float damage, Vector2 knockback, float damageDuration=0.15f)
    {
        m_state = State.Damaged;
        m_rigidbody.velocity = knockback;
        m_damageDuration = damageDuration;
    }
}
