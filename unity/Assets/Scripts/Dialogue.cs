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
    [SerializeField] private bool m_summonsPortal;
    public bool SummonsPortal { get => m_summonsPortal; }

    [SerializeField] private bool m_spawnsEnemies;
    public bool SpawnsEnemies { get => m_spawnsEnemies; }

    [SerializeField] private bool m_completesSequence;
    public bool CompletesSequence { get => m_completesSequence; }

    [SerializeField] private bool m_completesGame;
    public bool CompletesGame { get => m_completesGame; }

    [SerializeField] private bool m_restartsBattle;
    public bool RestartsBattle { get => m_restartsBattle; }

}