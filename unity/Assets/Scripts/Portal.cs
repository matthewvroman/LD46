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

    private bool m_suction = false;

    // Start is called before the first frame update
    void Start()
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

        if(direction.x <= m_distanceForFullSuction.x && Mathf.Abs(direction.y) <= m_distanceForFullSuction.y && m_suction == false)
        {
            StartCoroutine(SuckInPlayer());
            m_suction = true;
        }
    }

    IEnumerator SuckInPlayer()
    {
        m_playerController.Rigidbody.simulated = false;
        m_playerController.Renderer.sortingLayerName = "Foreground";

        Vector3 direction = this.transform.position - m_playerController.transform.position;
        float startDistance = direction.magnitude;
        float distance = startDistance;

        while(distance > 0.02f)
        {
            direction = this.transform.position - m_playerController.transform.position;
            distance = direction.magnitude;

            m_playerController.transform.Rotate(Vector3.forward * -720.0f * Time.deltaTime);
            m_playerController.transform.localScale = Vector3.one * distance/startDistance;
            m_playerController.transform.position = Vector3.Lerp(m_playerController.transform.position, this.transform.position, Time.deltaTime * 3.0f);

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("BattleScene");
        
    }
}
