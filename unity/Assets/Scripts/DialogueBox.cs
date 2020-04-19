using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private Button m_button;
    [SerializeField] private CanvasGroup m_canvasGroup;

    private DialogueResponse m_response;
    public DialogueResponse Response { get => m_response; }

    public Action<DialogueResponse> OnClickedResponse;
    public Action<DialogueResponse> OnClickedResponseAnimationComplete;
    public Action OnTextAnimationComplete;

    private Vector3 m_localDefaultPosition;

    private void Awake()
    {
        if(m_button) m_button.onClick.AddListener(OnButtonClicked);
        m_localDefaultPosition = this.transform.localPosition;
    }

    public void DisplayText(string text)
    {
        m_text.text = text;
        StartCoroutine(AnimateText(text, 0.35f));

        m_response = null;
    }

    public void DisplayResponse(DialogueResponse response)
    {
        m_response = response;
        m_text.text = response.Text;

    }

    private void OnButtonClicked()
    {
        Debug.Log("BUTTON CLICKED: " + m_response);
        if(OnClickedResponse != null) OnClickedResponse(m_response);
        StartCoroutine(AnimateClick());
    }

    private IEnumerator AnimateClick()
    {
        float duration = 0.1f;
        float timeElapsed = 0.0f;
        Vector3 initialScale = this.transform.localScale;
        Vector3 altScale = this.transform.localScale * 1.15f;
        while(timeElapsed<duration)
        {
            timeElapsed += Time.deltaTime;
            timeElapsed = Math.Min(timeElapsed, duration);
            this.transform.localScale = Vector3.Lerp(initialScale, altScale, Easing.EaseOutQuad(timeElapsed/duration));
            yield return new WaitForSeconds(Time.deltaTime);
        }

        timeElapsed = 0.0f;
        while(timeElapsed<duration)
        {
            timeElapsed += Time.deltaTime;
            timeElapsed = Math.Min(timeElapsed, duration);
            this.transform.localScale = Vector3.Lerp(altScale, initialScale, Easing.EaseOutQuad(timeElapsed/duration));
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(0.5f);

        if(OnClickedResponseAnimationComplete != null) OnClickedResponseAnimationComplete(m_response);
    }

    private IEnumerator AnimateText(string text, float delay, float timeBetweenCharacters=0.025f)
    {
        m_text.text = "";
        yield return new WaitForSeconds(delay);

        int currentCharacter = 0;
        int maxCharacters = text.Length;
        float timeUntilNextCharacters = timeBetweenCharacters;
        while(currentCharacter<maxCharacters)
        {
            timeUntilNextCharacters -= Time.deltaTime;
            if(timeUntilNextCharacters<=0.0f)
            {
                int numCharactersToWrite = 1 + (int)Math.Ceiling(Math.Abs(timeUntilNextCharacters/timeBetweenCharacters));
                if(numCharactersToWrite+currentCharacter>maxCharacters)
                {
                    numCharactersToWrite = maxCharacters-currentCharacter;
                }
                int index = GetPunctuationIndex(text.Substring(currentCharacter, numCharactersToWrite));
                if(index != -1)
                {
                    numCharactersToWrite = index+1;
                    timeUntilNextCharacters = 0.35f;
                }
                else
                {
                    timeUntilNextCharacters = timeBetweenCharacters;
                }
                
                currentCharacter += numCharactersToWrite;
                
                currentCharacter = Math.Min(currentCharacter, maxCharacters);

                string visible = text.Substring(0, currentCharacter);
                string invisible = "<color=#E9E9E900>" + text.Substring(currentCharacter, text.Length-currentCharacter) + "</color>";
                m_text.text = visible + invisible;

                
            }
            yield return new WaitForEndOfFrame();
        }

        if(OnTextAnimationComplete != null) OnTextAnimationComplete();
    }

    public void SetInteractable(bool interactable)
    {
        if(m_button) m_button.interactable = interactable;
    }

    public void Enter(float delay)
    {
        this.gameObject.SetActive(true);
        m_button.interactable = false;
        m_canvasGroup.alpha = 0;
        Vector3 from = m_localDefaultPosition;
        from.x += 200;
        StopAllCoroutines();
        StartCoroutine(TweenTo(from, m_localDefaultPosition, 0, 1, 0.35f, delay, ()=>
        {
            m_button.interactable = true;
        }));
    }

    public void Exit(float delay)
    {
        if(!this.gameObject.activeInHierarchy) return;

        Vector3 to = m_localDefaultPosition;
        //to.x -= 200;
        StopAllCoroutines();
        StartCoroutine(TweenTo(this.transform.localPosition, to, 1, 0, 0.35f, delay, ()=>
        {
            m_button.interactable = false;
            this.gameObject.SetActive(false);
        }));
    }

    private IEnumerator TweenTo(Vector3 fromPosition, Vector3 toPosition, float fromAlpha, float toAlpha, float duration, float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);

        float timeElapsed = 0.0f;
        while(timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            timeElapsed = Math.Min(timeElapsed, duration);
            this.transform.localPosition = Vector3.Lerp(fromPosition, toPosition, Easing.EaseOutQuad(timeElapsed/duration));
            if(m_canvasGroup) m_canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, Easing.EaseOutQuad(timeElapsed/duration));
            // this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, toPosition, timeElapsed/duration);
            // m_canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, timeElapsed/duration);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        this.transform.localPosition = toPosition;
        m_canvasGroup.alpha = toAlpha;

        if(onComplete != null) onComplete();        
    }

    private int GetPunctuationIndex(string text)
    {
        string[] punctuation = new string[]{".", "!", "?", ","};
        int minIndex=int.MaxValue;
        for(int i=0; i<punctuation.Length; i++)
        {
            int index = text.IndexOf(punctuation[i]);
            if(index != -1)
            {
                minIndex = Mathf.Min(index+punctuation[i].Length-1, minIndex);
            }
        }

        if(minIndex<int.MaxValue)
        {
            return minIndex;
        }
        return -1;
    }
}
