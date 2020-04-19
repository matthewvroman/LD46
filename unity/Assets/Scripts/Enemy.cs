using System;
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

    [SerializeField] private SpriteRenderer m_spriteRenderer;

    [SerializeField] private Vector3 m_spellCircleOffset;
    public Vector3 SpellCircleOffset { get => m_spellCircleOffset; }

    [SerializeField] private Vector3 m_healthBarOffset;
    public Vector3 HealthBarOffset { get => m_healthBarOffset; }

    [SerializeField] private float m_maxHealth;
    public float MaxHealth { get => m_maxHealth; }
    private float m_currentHealth;
    public float CurrentHealth { get => m_currentHealth; }
    public bool Dead { get => m_currentHealth <= 0.0f; }

    private State m_state;

    private PlayerController m_player;

    private float m_damageDuration;

    public Action OnDamaged;


    // Start is called before the first frame update
    void Awake()
    {
        m_player = GameObject.FindObjectOfType<PlayerController>();
        m_currentHealth = m_maxHealth;
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

        if(m_state == State.Dead)
        {
            this.transform.Rotate(this.transform.forward * 360.0f*Time.deltaTime);
            this.transform.localScale += Vector3.one * Time.deltaTime * 3.0f;
        
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
        if(m_state == State.Dead) return;

        m_state = State.Damaged;
        m_rigidbody.velocity = knockback;
        m_damageDuration = damageDuration;
        m_currentHealth -= damage;

        if(OnDamaged != null) OnDamaged();

        if(m_currentHealth <= 0)
        {
            Die(knockback);
        }
    }

    private void Die(Vector2 finalKnockback)
    {
        Vector2 impulse = finalKnockback * 2.0f; //increase knockback
        impulse.y += 5.0f;
        m_state = State.Dead;
        m_rigidbody.constraints = RigidbodyConstraints2D.None;
        m_rigidbody.gravityScale = 2.0f;
        m_rigidbody.AddForce(impulse, ForceMode2D.Impulse);
        m_spriteRenderer.sortingLayerName = "Foreground";
    }
}
