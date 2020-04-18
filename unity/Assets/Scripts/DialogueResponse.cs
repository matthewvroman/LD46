using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    [SerializeField] private string m_text;
    public string Text { get => m_text; }

    [SerializeField] private Dialogue m_dialogue;
    public Dialogue Dialogue { get => m_dialogue; }
}