using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private float m_floatDistance;
    [SerializeField] private Vector2 m_maxPullDistance;
    [SerializeField] private float m_pullStrength;
    [SerializeField] private Vector2 m_distanceForFullSuction;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private Loading m_loading;
    [SerializeField] private string m_sceneToLoad;

    private bool m_suction = false;
    [SerializeField] private bool m_allowSuction;

    // Start is called before the first frame update
    void Awake()
    {
    }

    void Update()
    {
        //float
        Vector3 position = this.transform.localPosition;
        position.y = Mathf.Sin(Time.realtimeSinceStartup) * m_floatDistance;
        this.transform.localPosition = position;
    }

    void FixedUpdate()
    {
        Vector3 direction = this.transform.position - m_playerController.transform.position;
        
        float distance = direction.magnitude;
        if(direction.x > 0 && direction.x <= m_maxPullDistance.x && Mathf.Abs(direction.y) < m_maxPullDistance.y)
        {
            //Vector2 velocity = m_playerController.Rigidbody.velocity;
            //velocity.x += direction.x * m_pullStrength; // * (1.0f-distance/m_maxPullDistance);
            //velocity.y += direction.y * m_pullStrength; // * (1.0f-distance/m_maxPullDistance);
            //m_playerController.Rigidbody.velocity = velocity;
        }

        if(direction.x <= m_distanceForFullSuction.x && Mathf.Abs(direction.y) <= m_distanceForFullSuction.y && m_suction == false && m_allowSuction)
        {
            StartCoroutine(SuckInPlayer());
            m_suction = true;
        }
    }
    
    public void Spawn()
    {
        StartCoroutine(SpawnCoroutine());
    }
    IEnumerator SpawnCoroutine()
    {
        this.transform.localScale = Vector3.zero;

        float elapsedTime = 0.0f;
        float duration = 0.5f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Min(elapsedTime, duration);
            this.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Easing.EaseOutBack(elapsedTime/duration));
            yield return new WaitForEndOfFrame();
        }
    }

    public void Despawn()
    {
        StartCoroutine(DespawnCoroutine());
    }

    IEnumerator DespawnCoroutine()
    {
        this.transform.localScale = Vector3.zero;

        float elapsedTime = 0.0f;
        float duration = 0.5f;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Min(elapsedTime, duration);
            this.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Easing.EaseOutQuad(elapsedTime/duration));
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Expel(GameObject objectToExpel)
    {
        float timeElapsed = 0.0f;
        float duration = 0.65f;

        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position;
        endPos.y -= 1.0f;
        endPos.x += 2;

        float startRotation = 540;
        float endRotation = 0;

        float startScale = 0.0f;
        float endScale = 1.0f;

        while(timeElapsed<duration)
        {
            timeElapsed += Time.deltaTime;
            timeElapsed = Mathf.Min(timeElapsed, duration);

            objectToExpel.transform.position = Vector3.Lerp(startPos, endPos, timeElapsed/duration);
            objectToExpel.transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, timeElapsed/duration);
            objectToExpel.transform.eulerAngles = Vector3.forward * Mathf.Lerp(startRotation, endRotation, timeElapsed/duration);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SuckInPlayer()
    {
        m_playerController.Rigidbody.simulated = false;
        m_playerController.Renderer.sortingLayerName = "Foreground";

        Vector3 direction = this.transform.position - m_playerController.transform.position;
        float startDistance = direction.magnitude;
        float distance = startDistance;

        while(distance > 0.15f)
        {
            direction = this.transform.position - m_playerController.transform.position;
            distance = direction.magnitude;

            m_playerController.transform.Rotate(Vector3.forward * -720.0f * Time.deltaTime);
            m_playerController.transform.localScale = Vector3.one * distance/startDistance;
            m_playerController.transform.position = Vector3.Lerp(m_playerController.transform.position, this.transform.position, Time.deltaTime * 3.0f);

            yield return new WaitForEndOfFrame();
        }

        Loading loading = GameObject.Instantiate(m_loading);
        loading.Load(m_sceneToLoad);
        
    }

    
}
