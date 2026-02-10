using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutPopupManager : MonoBehaviour
{
    [Header("Popup Reference")]
    [SerializeField] private GameObject aboutPopup;

    // Start is called before the first frame update
    void Start()
    {
        if (aboutPopup != null)
            aboutPopup.SetActive(false);
    }

    public void OnStartButtonClick()
    {
        if (GameSceneManager.Instance != null)
            GameSceneManager.Instance.LoadGameScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAboutPopup()
    {
        if (aboutPopup != null)
            aboutPopup.SetActive(true);
    }

    public void CloseAboutPopup()
    {
        if (aboutPopup != null)
            aboutPopup.SetActive(false);
    }
}
