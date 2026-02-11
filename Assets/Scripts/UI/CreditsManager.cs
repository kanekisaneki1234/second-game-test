using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private ScrollRect creditsScrollView;
    [SerializeField] private float autoScrollSpeed = 5f;
    [SerializeField] private bool autoScroll = true;
    
    [Header("Auto Return Settings")]
    [SerializeField] private bool autoReturnToMenu = true;
    [SerializeField] private float autoReturnDelay = 5f;
    
    private bool scrollComplete = false;
    private float autoReturnTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (autoScroll && creditsScrollView != null)
        {
            StartCoroutine(AutoScrollCredits());
        }
        
        autoReturnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            ReturnToMainMenu();
        }
        
        if (autoReturnToMenu && scrollComplete)
        {
            autoReturnTimer += Time.deltaTime;
            if (autoReturnTimer >= autoReturnDelay)
            {
                ReturnToMainMenu();
            }
        }
    }

    private IEnumerator AutoScrollCredits()
    {
        if (creditsScrollView == null)
        {
            Debug.Log("Check Failed");
            yield break;
        }
        
        creditsScrollView.verticalNormalizedPosition = 1f;
        
        yield return new WaitForSeconds(2f);
        
        while (creditsScrollView.verticalNormalizedPosition > 0f)
        {
            creditsScrollView.verticalNormalizedPosition -= autoScrollSpeed * Time.deltaTime / 100f;
            yield return null;
        }
        
        creditsScrollView.verticalNormalizedPosition = 0f;
        scrollComplete = true;
        
        Debug.Log("Credits scroll complete");
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu from Credits");
        
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.LoadMainMenu();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

    public void SkipToEnd()
    {
        if (creditsScrollView != null)
        {
            StopAllCoroutines();
            creditsScrollView.verticalNormalizedPosition = 0f;
            scrollComplete = true;
        }
    }
}
