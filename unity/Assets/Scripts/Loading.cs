using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private float m_minLoadTime = 3.0f;
    [SerializeField] private CanvasGroup m_canvasGroup;
    private bool m_hideRequested;
    private bool m_hiding;
    
    private float m_timeElapsed;

    [SerializeField] private string m_sceneToLoadOnAwake;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(!string.IsNullOrEmpty(m_sceneToLoadOnAwake))
        {
            Load(m_sceneToLoadOnAwake);
        }
    }

    public void Show()
    {
        m_timeElapsed = 0.0f;
        m_canvasGroup.alpha = 0.0f;
        m_hiding = false;
    }

    public void Load(string scene)
    {
        Show();
        StartCoroutine(LoadScene(scene));
    }

    private IEnumerator LoadScene(string scene)
    {
        yield return new WaitForSeconds(0.35f); //artificial delay for fade

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while(asyncLoad.isDone == false)
        {
            yield return null;
        }
        Hide();
    }

    public void Hide()
    {
        m_hideRequested = true;
    }
    

    private void Update()
    {
        m_timeElapsed += Time.deltaTime;

        if(m_hideRequested && m_timeElapsed >= m_minLoadTime && !m_hiding)
        {
            m_hiding = true;
        }
        
        float finalValue = m_hiding?0:1;
        m_canvasGroup.alpha = Mathf.Lerp(m_canvasGroup.alpha, finalValue, Time.deltaTime * 10.0f);

        if(m_hiding && m_canvasGroup.alpha<=0.1f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
