using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    public void Show(Vector3 position)
    {
        this.gameObject.SetActive(true);
        this.transform.localEulerAngles = Vector3.forward * UnityEngine.Random.Range(0, 180);
        this.transform.localPosition = position;
        m_animator.Play("Hit", 0);
    }

    private void Update()
    {
        if(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1)
        {
            this.gameObject.SetActive(false);
        }
    }
}
