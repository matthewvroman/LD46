using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "ScriptableObjects/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
    [SerializeField] private string m_name;
    public string Name { get => m_name; }

    [SerializeField] private Sprite m_displaySprite;
    public Sprite DisplaySprite { get => m_displaySprite; }

    [SerializeField] private Vector2 m_displayOffset;
    public Vector2 DisplayOffset { get => m_displayOffset; }

    [SerializeField] private Dialogue m_dialogue;
    public Dialogue Dialogue { get => m_dialogue; }

}
