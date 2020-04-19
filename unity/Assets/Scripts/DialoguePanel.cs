using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    private enum State
    {
        Entering,
        Default,
        Exiting
    }

    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Image m_characterImage;
    [SerializeField] private DialogueBox m_dialogueBox;
    [SerializeField] private DialogueBox[] m_responseBoxes;
    [SerializeField] private GameObject m_gameComplete;

    private DialogueCharacter m_character;

    private Dialogue m_dialogue;

    private Vector3 m_localExitPosition = new Vector2(-250, 0);

    private State m_state = State.Exiting;

    PlayerController m_playerController;

    private static bool m_inDialogue;
    public static bool InDialogue { get => m_inDialogue; }

    private void Awake()
    {
        m_dialogueBox.OnTextAnimationComplete += OnTextAnimationComplete;
        for(int i=0; i<m_responseBoxes.Length; i++)
        {
            m_responseBoxes[i].OnClickedResponse += OnClickedResponse;
            m_responseBoxes[i].OnClickedResponseAnimationComplete += OnClickedResponseAnimationComplete;
        }
    }

    private void OnEnable()
    {
        m_inDialogue = true;
    }

    private void OnDisable()
    {
        m_inDialogue = false;
    }

    public void Display(DialogueCharacter character, Dialogue dialogue)
    {
        this.gameObject.SetActive(true);

        if(m_state == State.Exiting)
        {
            m_state = State.Entering;
            m_characterImage.transform.localPosition = m_localExitPosition;
            m_canvasGroup.alpha = 0.0f;

            for(int i=0; i<m_responseBoxes.Length; i++)
            {
                DialogueBox responseBox = m_responseBoxes[i];
                responseBox.gameObject.SetActive(false);
            }
        }

        m_character = character;
        m_characterImage.sprite = character.DisplaySprite;
        m_characterImage.SetNativeSize();

        m_dialogue = dialogue;

        m_dialogueBox.DisplayText(m_dialogue.Text);
    }

    private void OnTextAnimationComplete()
    {
        for(int i=0; i<m_responseBoxes.Length; i++)
        {
            DialogueBox responseBox = m_responseBoxes[i];
            if(i<m_dialogue.Responses.Length)
            {
                responseBox.DisplayResponse(m_dialogue.Responses[i]);
                responseBox.Enter(i*0.125f);
            }
        }
    }

    private void OnClickedResponse(DialogueResponse response)
    {
        if(m_state != State.Default)
        {
            return;
        }

        for(int i=0; i<m_responseBoxes.Length; i++)
        {
            DialogueBox responseBox = m_responseBoxes[i];
            responseBox.SetInteractable(false);
            if(responseBox.Response != response)
            {
                responseBox.Exit(0.0f);
            }
        }
    }

    private void OnClickedResponseAnimationComplete(DialogueResponse response)
    {
        for(int i=0; i<m_responseBoxes.Length; i++)
        {
            DialogueBox responseBox = m_responseBoxes[i];
            if(responseBox.Response == response)
            {
                responseBox.Exit(0.125f);
            }
        }

        if(m_dialogue.SummonsPortal)
        {
            //SUMMON PORTAL!
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "BasementScene")
            {
                BasementManager.Instance.SpawnPortal();
            }
            else
            {
                BattleManager.Instance.SpawnPortal();
            }
        }
        if(m_dialogue.SpawnsEnemies)
        {
            BattleManager.Instance.SpawnEnemies();
        }
        m_character.CompletedSequence = m_dialogue.CompletesSequence;
        if(m_dialogue.CompletesGame)
        {
            m_gameComplete.SetActive(true);
        }

        if(response.Dialogue)
        {
            Display(m_character, response.Dialogue);
        }
        else
        {
            Exit();
        }  
    }

    public void Exit()
    {
        m_state = State.Exiting;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_state == State.Entering)
        {
            //tweens
            m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, 1, Time.deltaTime * 5.0f);
            
            m_characterImage.transform.localPosition = Vector2.Lerp(m_characterImage.transform.localPosition, m_character.DisplayOffset, Time.deltaTime * 5.0f);

            if(m_canvasGroup.alpha > 0.98f) //hacky
            {
                m_state = State.Default;
            }
        }
        else if(m_state == State.Exiting)
        {
             m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, 0, Time.deltaTime * 10.0f);
             m_characterImage.transform.localPosition = Vector2.Lerp(m_characterImage.transform.localPosition, m_localExitPosition, Time.deltaTime * 10.0f);

             if(m_canvasGroup.alpha < 0.02f) //hacky
             {
                this.gameObject.SetActive(false);
             }
        }
    }
}
