using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private ScrollRect creditsScrollView;
    // [SerializeField] private float scrollDuration = 40f;
    [SerializeField] private float autoScrollSpeed = 5f;
    [SerializeField] private bool autoScroll = true;
    
    [Header("Auto Return Settings")]
    [SerializeField] private bool autoReturnToMenu = true;
    [SerializeField] private float autoReturnDelay = 30f; // 30 seconds
    
    private bool scrollComplete = false;
    private float autoReturnTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (autoScroll && creditsScrollView != null)
        {
            StartCoroutine(AutoScrollCredits());
        }
        
        // Reset timer
        autoReturnTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            ReturnToMainMenu();
        }
        
        // Auto-return timer
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
        // if (creditsScrollView == null) yield break;
        if (creditsScrollView == null)
        {
            Debug.Log("Check Failed");
            yield break;
        }
        
        // Start at top
        creditsScrollView.verticalNormalizedPosition = 1f;
        
        // Wait a moment before starting
        yield return new WaitForSeconds(2f);
        
        // Scroll down smoothly
        while (creditsScrollView.verticalNormalizedPosition > 0f)
        {
            creditsScrollView.verticalNormalizedPosition -= autoScrollSpeed * Time.deltaTime / 100f;
            yield return null;
        }
        
        // Ensure we're at the bottom
        creditsScrollView.verticalNormalizedPosition = 0f;
        scrollComplete = true;
        
        Debug.Log("Credits scroll complete");
    }

    // private IEnumerator AutoScrollCredits()
    // {
    //     if (creditsScrollView == null)
    //     {
    //         Debug.LogError("CreditsScrollView is null!");
    //         yield break;
    //     }
        
    //     Debug.Log("Starting auto-scroll");
        
    //     // Get dimensions to calculate actual bottom
    //     RectTransform content = creditsScrollView.content;
    //     RectTransform viewport = creditsScrollView.viewport;
        
    //     float contentHeight = content.rect.height;
    //     float viewportHeight = viewport.rect.height;
    //     float scrollableHeight = contentHeight - viewportHeight;
        
    //     Debug.Log($"Content: {contentHeight}, Viewport: {viewportHeight}, Scrollable: {scrollableHeight}");
        
    //     if (scrollableHeight <= 0)
    //     {
    //         Debug.LogWarning("Content is not taller than viewport!");
    //         scrollComplete = true;
    //         yield break;
    //     }
        
    //     content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0f);
        
    //     yield return new WaitForSeconds(3f);
        
    //     Debug.Log($"Scrolling for {scrollDuration} seconds...");
        
    //     float elapsed = 0f;
        
    //     while (elapsed < scrollDuration)
    //     {
    //         elapsed += Time.deltaTime;
            
    //         float progress = elapsed / scrollDuration;
            
    //         float newY = Mathf.Lerp(0f, scrollableHeight, progress);
    //         content.anchoredPosition = new Vector2(content.anchoredPosition.x, newY);
            
    //         yield return null;
    //     }
        
    //     // Ensure we're at the exact bottom
    //     content.anchoredPosition = new Vector2(content.anchoredPosition.x, scrollableHeight);
        
    //     Debug.Log("Reached bottom after 40 seconds");
        
    //     yield return new WaitForSeconds(3f);
        
    //     scrollComplete = true;
    //     Debug.Log("Scroll complete");
    // }

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
