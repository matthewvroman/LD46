﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State
    {
        Move,
        Attack,
        Cast
    }
    [SerializeField] private float m_speed;
    [SerializeField] private float m_verticalSpeed;
    [SerializeField] private Rigidbody2D m_rigidbody;
    public Rigidbody2D Rigidbody { get => m_rigidbody; }
    [SerializeField] private Animator m_animator;
    [SerializeField] private SpriteRenderer m_renderer;
    public SpriteRenderer Renderer { get => m_renderer; }
    [SerializeField] private float m_desiredDirection;
    public float DesiredDirection { get => m_desiredDirection; }
    [SerializeField] private Rect m_bounds;
    [SerializeField] private int m_numAttacks = 4;
    [SerializeField] private float m_maxTimeForAttackChain;
    [SerializeField] private HitEffect m_hitEffect;
    [SerializeField] private Vector3[] m_hitPositions;
    [SerializeField] private Spell[] m_spells;
    [SerializeField] private ContactFilter2D m_attackContactFilter;

    private State m_state;
    private int m_attackIndex;
    private float m_lastAttackTime;

    private CameraEffects m_cameraEffects;
    

    // Start is called before the first frame update
    void Start()
    {
        m_state = State.Move;
        m_desiredDirection = 1;

        m_cameraEffects = GameObject.FindObjectOfType<CameraEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = m_animator.gameObject.transform.localScale;
        scale.x = Mathf.Lerp(scale.x, m_desiredDirection, Time.deltaTime * 15.0f);
        m_animator.gameObject.transform.localScale = scale;

        if(Input.GetKeyDown(KeyCode.Q) && m_state == State.Move)
        {
            StartCoroutine(Attack());
        }
        if(Input.GetKeyDown(KeyCode.E) && m_state == State.Move)
        {
            StartCoroutine(Cast());
        }
    }

    private IEnumerator Cast()
    {
        m_state = State.Cast;
        m_animator.SetBool("Casting", true);
        m_animator.SetTrigger("StartCast");
        m_rigidbody.velocity = Vector2.zero;

        m_spells[0].MarkTargets(this);
        yield return new WaitForSeconds(0.85f);
        m_spells[0].Cast(this);

        yield return new WaitForSeconds(0.4f);
        m_animator.SetBool("Casting", false);
        m_state = State.Move;
    }

    private IEnumerator Attack()
    {
        m_state = State.Attack;
        if(Time.realtimeSinceStartup - m_lastAttackTime <= m_maxTimeForAttackChain)
        {
            m_attackIndex++;
            m_attackIndex%=(m_numAttacks);
        }
        else
        {
            m_attackIndex = 0;
        }
        m_animator.SetInteger("AttackIndex", m_attackIndex);
        m_animator.SetBool("Running", false);
        m_animator.SetBool("Attacking", true);
        Vector2 velocity = m_rigidbody.velocity;
        velocity.x = 0.0f;
        m_rigidbody.velocity = velocity;
        m_rigidbody.AddForce(transform.right * 2.5f * m_desiredDirection, ForceMode2D.Impulse);
        m_lastAttackTime = Time.realtimeSinceStartup;

        Vector3 hitPosition = m_hitPositions[m_attackIndex];
        if(m_desiredDirection < 0) hitPosition.x = -hitPosition.x;

        List<RaycastHit2D> results = new List<RaycastHit2D>();
        float size = 0.5f;
        Physics2D.BoxCast(this.transform.position + hitPosition, new Vector2(size,size), 0, transform.right, m_attackContactFilter.NoFilter(), results, size);
        bool hitEnemy = false;
        for(int i=0; i<results.Count; i++)
        {
            Enemy enemy = results[i].transform.gameObject.GetComponentInChildren<Enemy>();
            if(enemy != null)
            {
                enemy.Damage(0, Vector2.right * m_desiredDirection * 2.5f);
                hitEnemy = true;
            }
            
        }
        
        if(hitEnemy)
        {
            m_hitEffect.Show(hitPosition);
            m_cameraEffects.Shake(Vector2.one * 0.1f, 0.1f);
        }

        yield return new WaitForSeconds(0.15f);
        m_animator.SetBool("Attacking", false);
        m_state = State.Move;
    }

    void FixedUpdate()
    {
        switch(m_state)
        {
            case State.Move:
                MoveFixedUpdate();
                break;   
        }
    }

    void MoveFixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //direction
        m_animator.SetBool("Running", horizontal != 0 || vertical != 0);
        if(horizontal != 0)
        {
            m_desiredDirection = horizontal<0?-1:1;
        }

        if(this.transform.localPosition.y > m_bounds.yMax && vertical > 0)
        {
            vertical = 0;
        }
        else if(this.transform.localPosition.y < m_bounds.yMin && vertical < 0)
        {
            vertical = 0;
        }

        if(this.transform.localPosition.x > m_bounds.xMax && horizontal > 0)
        {
            horizontal = 0;
        }
        else if(this.transform.localPosition.x < m_bounds.xMin && horizontal < 0)
        {
            horizontal = 0;
        }

        m_rigidbody.velocity = new Vector2(horizontal * m_speed, vertical * m_verticalSpeed);
    }

    public void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(new Vector2(m_bounds.x, m_bounds.y), new Vector2(m_bounds.x + m_bounds.width, m_bounds.y)); //top
        Gizmos.DrawLine(new Vector2(m_bounds.x, m_bounds.y), new Vector2(m_bounds.x, m_bounds.y+m_bounds.height)); //left
        Gizmos.DrawLine(new Vector2(m_bounds.x, m_bounds.y+m_bounds.height), new Vector2(m_bounds.x + m_bounds.width, m_bounds.y+m_bounds.height)); //bottom
        Gizmos.DrawLine(new Vector2(m_bounds.x + m_bounds.width, m_bounds.y), new Vector2(m_bounds.x + m_bounds.width, m_bounds.y+m_bounds.height)); //right
	}
}
