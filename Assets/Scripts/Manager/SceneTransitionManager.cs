using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    [Header("UI References")]
    [SerializeField] private Canvas transitionCanvas;
    [SerializeField] private Image fadeImage;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;
    
    [Header("Auto Fade In On Start")]
    [SerializeField] private bool fadeInOnSceneLoad = true;
    
    private bool isTransitioning = false;

    private static int instanceCount = 0;
    // Start is called before the first frame update

    void Awake()
    {
        instanceCount++;
        Debug.Log($"Instance count: {instanceCount}");
        // Singleton with DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (transitionCanvas != null)
            {
                DontDestroyOnLoad(transitionCanvas.gameObject);
            }
        }
        // else
        // {
        //     Debug.Log("Destroyed Duplicate Instance");
        //     Destroy(gameObject);
        //     return;
        // }
        
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
        }
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Fade in on first load
        if (fadeInOnSceneLoad)
        {
            StartCoroutine(FadeIn());
        }

        if (Instance != this)
        {
            // This is a duplicate - destroy after a delay to allow fade to complete
            Destroy(gameObject, 2f); 
            instanceCount--;
            Debug.Log(instanceCount);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeInOnSceneLoad && !isTransitioning)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene(sceneName));
        }
    }

    // public void LoadSceneWithFade(int sceneIndex)
    // {
    //     if (!isTransitioning)
    //     {
    //         StartCoroutine(TransitionToScene(sceneIndex));
    //     }
    // }

    private IEnumerator TransitionToScene(string sceneName)
    {
        isTransitioning = true;
        
        Debug.Log($"Fading out and loading: {sceneName}");
        
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Load scene
        SceneManager.LoadScene(sceneName);
        
        // Fade in happens automatically via OnSceneLoaded
        
        isTransitioning = false;
    }

    // private IEnumerator TransitionToScene(int sceneIndex)
    // {
    //     isTransitioning = true;
        
    //     Debug.Log($"Fading out and loading scene index: {sceneIndex}");
        
    //     // Fade out
    //     yield return StartCoroutine(FadeOut());
        
    //     // Load scene
    //     SceneManager.LoadScene(sceneIndex);
        
    //     // Fade in happens automatically via OnSceneLoaded
        
    //     isTransitioning = false;
    // }

    public IEnumerator FadeOut()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is null!");
            yield break;
        }
        
        Debug.Log("Fading out...");
        
        // Ensure canvas is active and on top
        if (transitionCanvas != null)
        {
            transitionCanvas.gameObject.SetActive(true);
            transitionCanvas.sortingOrder = 9999; // Render on top of everything
        }
        
        float elapsed = 0f;
        Color startColor = fadeImage.color;
        startColor.a = 0f;
        Color endColor = fadeColor;
        endColor.a = 1f;
        
        fadeImage.color = startColor;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time (works even if game is paused)
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            
            Color newColor = fadeColor;
            newColor.a = alpha;
            fadeImage.color = newColor;
            
            yield return null;
        }
        
        // Ensure fully opaque
        endColor.a = 1f;
        fadeImage.color = endColor;
        
        Debug.Log("Fade out complete");
    }

    public IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is null!");
            yield break;
        }
        
        Debug.Log("Fading in...");
        
        // Ensure canvas is active
        if (transitionCanvas != null)
        {
            transitionCanvas.gameObject.SetActive(true);
            transitionCanvas.sortingOrder = 9999;
        }
        
        float elapsed = 0f;
        Color startColor = fadeColor;
        startColor.a = 1f;
        Color endColor = fadeColor;
        endColor.a = 0f;
        
        fadeImage.color = startColor;
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            
            Color newColor = fadeColor;
            newColor.a = alpha;
            fadeImage.color = newColor;
            
            yield return null;
        }
        
        // Ensure fully transparent
        endColor.a = 0f;
        fadeImage.color = endColor;
        
        Debug.Log("Fade in complete");
        
        // Optionally deactivate canvas when not fading
        // if (transitionCanvas != null)
        //     transitionCanvas.gameObject.SetActive(false);
    }

    public void SetFadeBlack()
    {
        if (fadeImage != null)
        {
            Color color = fadeColor;
            color.a = 1f;
            fadeImage.color = color;
        }
    }

    public void SetFadeClear()
    {
        if (fadeImage != null)
        {
            Color color = fadeColor;
            color.a = 0f;
            fadeImage.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
