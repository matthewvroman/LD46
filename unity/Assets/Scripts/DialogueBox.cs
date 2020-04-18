using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private Button m_button;

    private DialogueResponse m_response;

    public Action<DialogueResponse> OnClickedResponse;

    private void Awake()
    {
        if(m_button) m_button.onClick.AddListener(OnButtonClicked);
    }

    public void DisplayText(string text)
    {
        m_text.text = text;
        //todo animate in

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
    }
}
