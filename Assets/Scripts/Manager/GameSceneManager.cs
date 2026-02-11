using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameScene = "GameScene";
    [SerializeField] private string creditsScene = "Credits";


    [Header("Transition Settings")]
    [SerializeField] private float transitionDelay = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        switch (scene.name)
        {
            case "MainMenu":
                PlayMusic(musicSource.clip);
                break;
            
            case "GameScene":
                musicSource.Stop();
                break;
            
            case "Credits":
                PlayMusic(musicSource.clip);
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;
        
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
        
        Debug.Log($"Playing music: {clip.name}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu...");
        LoadScene(mainMenuScene);
    }

    public void LoadGameScene()
    {   
        LoadScene(gameScene);
    }

    public void LoadCredits()
    {
        Debug.Log("Loading Credits...");
        LoadScene(creditsScene);
    }

    private void LoadScene(string sceneName)
    {
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadSceneWithFade(sceneName);
        }
        else
        {
            // Fallback: Direct load without transition
            Debug.LogWarning("SceneTransition not found, loading directly");
            
            if (transitionDelay > 0)
            {
                StartCoroutine(LoadSceneWithDelay(sceneName, transitionDelay));
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
