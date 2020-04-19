using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    public enum State
    {
        Moving,
        Attacking,
        Damaged,
        Dead,
        Retreating
    }
    [SerializeField] protected float m_speed;

    [SerializeField] protected Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody { get => m_rigidbody; }

    [SerializeField] protected SpriteRenderer m_spriteRenderer;

    [SerializeField] protected Animator m_animator;

    [SerializeField] protected Vector3 m_spellCircleOffset;
    public Vector3 SpellCircleOffset { get => m_spellCircleOffset; }

    [SerializeField] protected Vector3 m_healthBarOffset;
    public Vector3 HealthBarOffset { get => m_healthBarOffset; }

    [SerializeField] protected float m_maxHealth;
    public float MaxHealth { get => m_maxHealth; }
    protected float m_currentHealth;
    public float CurrentHealth { get => m_currentHealth; }
    public bool Dead { get => m_currentHealth <= 0.0f; }

    [SerializeField] protected HealthBar m_healthBar;

    protected State m_state;
    public State EnemyState { get => m_state; }

    protected PlayerController m_player;

    protected float m_damageDuration;

    public Action OnDamaged { get; set; }

    protected int m_desiredDirection;

    private float m_destroyAfterDeath = 5.0f;

    [SerializeField] protected float m_baseDamage;

    [SerializeField] protected ContactFilter2D m_attackContactFilter;
    [SerializeField] private int m_experience;
    public int Experience { get => m_experience; }

    public static Action<Enemy>Killed;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        m_player = GameObject.FindObjectOfType<PlayerController>();
        m_currentHealth = m_maxHealth;
        m_healthBar.SetInterface(this);
    }

    // Update is called once per frame
    public virtual void Update()
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
            m_destroyAfterDeath -= Time.deltaTime;
            if(m_destroyAfterDeath <= 0.0f)
            {
                GameObject.Destroy(this.gameObject);
            }
        }

        if(m_state == State.Moving || m_state == State.Retreating)
        {
            if(m_rigidbody.velocity.x != 0)
            {
                m_desiredDirection = m_rigidbody.velocity.x < 0? 1:-1;
            }

            Vector3 scale = m_animator.gameObject.transform.localScale;
            scale.x = Mathf.Lerp(scale.x, m_desiredDirection, Time.deltaTime * 15.0f);
            m_animator.gameObject.transform.localScale = scale;
        }
    }

    protected virtual void FixedUpdate()
    {
        if(m_state == State.Moving)
        {
            m_rigidbody.velocity = Vector2.zero;
        }
    }

    public virtual void Damage(float damage, Vector2 knockback, float damageDuration=0.15f)
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
        if(Killed != null) Killed(this);
    }

    public void Augment(float healthModifier, int expModifier, float speedModifier, float damageModifier)
    {
        m_maxHealth += healthModifier;
        m_currentHealth = m_maxHealth; 
        m_healthBar.Reset();
        m_healthBar.SetInterface(this);
        m_experience += expModifier;
        m_baseDamage += damageModifier;
    }
}
