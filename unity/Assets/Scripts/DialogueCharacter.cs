using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "ScriptableObjects/DialogueCharacter")]
public class DialogueCharacter : ScriptableObject
{
    [Serializable]
    public struct LevelPair
    {
        public int level;
        public Dialogue[] initialDialogues;
        public Dialogue[] completedSequenceDialogues;
    }
    [SerializeField] private string m_name;
    public string Name { get => m_name; }

    [SerializeField] private Sprite m_displaySprite;
    public Sprite DisplaySprite { get => m_displaySprite; }

    [SerializeField] private Vector2 m_displayOffset;
    public Vector2 DisplayOffset { get => m_displayOffset; }

    public Dialogue Dialogue 
    { 
        get
        {
            int index = LevelManager.Instance.Level-1;
            LevelPair pair = m_levelPairs[index];
            if(m_completedSequence)
            {
                return pair.completedSequenceDialogues[UnityEngine.Random.Range(0, pair.completedSequenceDialogues.Length-1)];
            }
            else
            {
                return pair.initialDialogues[UnityEngine.Random.Range(0, pair.initialDialogues.Length-1)];
            }
        }
    }
    [System.NonSerialized] private bool m_completedSequence = false;
    public bool CompletedSequence { get => m_completedSequence; set => m_completedSequence = value; }

    [SerializeField] private LevelPair[] m_levelPairs;

}
