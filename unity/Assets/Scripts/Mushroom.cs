using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    [SerializeField] private float m_attackRange;
    [SerializeField] private float m_timeBetweenAttacks = 1.0f;
    private float m_timeUntilNextAttack;
    [SerializeField] private float m_damagePerAttack;
    [SerializeField] private AnimationEventDispatcher m_eventDispatcher;
    [SerializeField] private Vector3 m_desiredPositionOffset;
    private float m_retreatTime;

    protected override void Awake()
    {
        base.Awake();
        m_eventDispatcher.OnEventReceived += OnEventReceived;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(m_state == Enemy.State.Moving)
        {
            Vector3 distance = m_player.transform.position - this.gameObject.transform.position;
            Vector3 offset = m_desiredPositionOffset;
            offset.x *= m_desiredDirection;
            distance += offset;

            if(distance.x > m_attackRange*2.0f)
            {
                distance.y = 0.0f; //don't even worry about getting on the players Y plane until we're close
            }

            m_rigidbody.velocity = distance.normalized * m_speed;

            m_timeUntilNextAttack -= Time.deltaTime;
            if(m_timeUntilNextAttack<=0)
            {
                if(distance.magnitude <= m_attackRange)
                {
                    Attack();
                }
                m_timeUntilNextAttack = m_timeBetweenAttacks;
            }
        }
        else if(m_state == Enemy.State.Retreating)
        {
            Vector3 distance = m_player.transform.position - this.gameObject.transform.position;

            Vector2 velocity = distance.normalized * m_speed;
            velocity.x = -velocity.x; //run away
            m_rigidbody.velocity = velocity;

            m_retreatTime -= Time.deltaTime;
            if(m_retreatTime <= 0)
            {
                m_state = Enemy.State.Moving;
            }
        }
    }

    public override void Update()
    {
        base.Update();

        m_animator.SetBool("Attack", m_state == State.Attacking);
    }

    private void Attack()
    {
        m_animator.SetBool("Attack", true);
        m_state = State.Attacking;

        List<RaycastHit2D> results = new List<RaycastHit2D>();
        float size = 0.75f;
        Physics2D.BoxCast(this.transform.position, new Vector2(size,size), 0, transform.right, m_attackContactFilter.NoFilter(), results, size);
        bool hitEnemy = false;
        for(int i=0; i<results.Count; i++)
        {
            PlayerController enemy = results[i].transform.gameObject.GetComponentInChildren<PlayerController>();
            if(enemy != null)
            {
                enemy.Damage(m_baseDamage, Vector2.right * m_desiredDirection * 2.5f);
            }
            
        }
    }

    private void OnEventReceived(string data)
    {
        if(data=="DealDamage")
        {
            DealDamage();
        }
        else if(data=="EndAttack")
        {
            EndAttack();
        }
    }

    public void DealDamage()
    {
        
    }

    public void EndAttack()
    {
        if(m_state == State.Attacking)
        {
            m_state = State.Retreating; 
            m_retreatTime = UnityEngine.Random.Range(1.0f, 4.0f);
        }
    }
}
