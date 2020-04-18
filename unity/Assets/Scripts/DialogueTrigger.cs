using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_speechBubble;
    [SerializeField] private DialogueCharacter m_character;
    [SerializeField] private DialoguePanel m_dialoguePanel;

    private Vector3 m_endingLocalPosition;
    private Vector3 m_startingLocalPosition;

    private bool m_inTrigger = false;

    private float m_alphaSpeed = 5.0f;
    private float m_positionSpeed = 5.0f;

    private void Awake()
    {
        m_endingLocalPosition = m_speechBubble.transform.localPosition;
        m_startingLocalPosition = m_endingLocalPosition;
        m_startingLocalPosition.y -= 0.25f;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<PlayerController>() == null) return;

        m_inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.GetComponent<PlayerController>() == null) return;
        
        m_inTrigger = false;
    }

    private void Update()
    {
        Vector3 desiredLocalPosition = m_inTrigger?m_endingLocalPosition:m_startingLocalPosition;
        float desiredAlpha = m_inTrigger?1:0;
        m_speechBubble.transform.localPosition = Vector3.Lerp(m_speechBubble.transform.localPosition, desiredLocalPosition, Time.deltaTime * m_positionSpeed);
        
        Color color = m_speechBubble.color;
        color.a = Mathf.Lerp(color.a, desiredAlpha, Time.deltaTime * m_alphaSpeed);
        m_speechBubble.color = color;

        if(m_inTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            if(!m_dialoguePanel.gameObject.activeInHierarchy)
            {
                m_dialoguePanel.Display(m_character, m_character.Dialogue);
            }
            else
            {
                m_dialoguePanel.Exit();
            }
        }
    }
}
