using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAssist : MonoBehaviour
{
    protected Spell m_spell;
    public Spell Spell { get => m_spell; }

    protected Enemy m_target;

    [SerializeField] private bool m_handlesDamage;
    public bool HandlesDamage { get { return m_handlesDamage; } }

    public virtual void SetSpell(Spell spell, Enemy target)
    {
        m_spell = spell;
        m_target = target;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
