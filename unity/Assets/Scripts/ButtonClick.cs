using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    private void OnButtonClick()
    {
        AudioManager.Instance.PlayButtonSfx();
    }
}
