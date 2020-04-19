using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfGameComplete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(LevelManager.Instance.GameComplete)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
