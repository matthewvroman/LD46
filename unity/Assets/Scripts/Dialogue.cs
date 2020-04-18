using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private string m_text;
    public string Text { get=> m_text; } 

    [SerializeField] private DialogueResponse[] m_responses;
    public DialogueResponse[] Responses { get => m_responses; }

    //todo prereq bools
}