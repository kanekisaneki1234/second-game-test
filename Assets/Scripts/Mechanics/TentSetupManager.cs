using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TentSetupManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip stakeSound;
    [SerializeField] private AudioClip completeSound;
    private AudioSource audioSource;
    
    [Header("UI References")]
    [SerializeField] private GameObject tentSetupPanel;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private Button[] stakeButtons;

    [Header("Tent GameObject Reference")]
    [SerializeField] private GameObject tentObject;
    [SerializeField] private SpriteRenderer tentSpriteRenderer;

    [Header("Tent Stage Sprites")]
    [SerializeField] private Sprite tentStage1_FlatOnGround;
    [SerializeField] private Sprite tentStage2_PolesRaised;
    [SerializeField] private Sprite tentStage3_Complete;

    private enum TentPhase
    {
        PlacingStakes,
        RaisingPoles,
        FinalSetup
    }

    [SerializeField] private TentPhase currentPhase = TentPhase.PlacingStakes;

    [Header("Tent Setup Settings")]
    [SerializeField] private int totalStakes = 4;
    private int stakesPlaced = 0;
    
    [Header("Phase Instructions")]
    [SerializeField] private string[] phase1Instructions = new string[]
    {
        "Phase 1: Stake Placement\n\nClick the 4 corners to place tent stakes.",
        "Good! Stake 1 placed at correct angle (45 degrees)",
        "Excellent! Always stake opposite corners for balance.",
        "Perfect! Stakes secure the tent against wind.",
        "All stakes placed! Ready to raise the tent."
    };
    
    [SerializeField] private string[] phase2Instructions = new string[]
    {
        "Phase 2: Raise Tent Poles\n\nClick to insert poles and raise the tent structure.",
        "Poles connected! The tent is taking shape.",
        "Tent raised successfully! Now for final touches."
    };
    
    [SerializeField] private string[] phase3Instructions = new string[]
    {
        "Phase 3: Final Setup\n\nSecure rainfly and guy lines.",
        "Rainfly attached! Protection from rain and wind.",
        "Setup Complete! You've learned proper tent pitching!"
    };
    
    private bool isSetupActive = false;
    private int phase2Progress = 0;
    private int phase3Progress = 0;

    // Coroutine reference for cancellation if needed
    private Coroutine phaseTransitionCoroutine;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (tentSetupPanel != null)
            tentSetupPanel.SetActive(false);

        if (tentSpriteRenderer != null && tentStage1_FlatOnGround != null)
            tentSpriteRenderer.sprite = tentStage1_FlatOnGround;
    }

    public void StartTentSetup()
    {
        isSetupActive = true;
        tentSetupPanel.SetActive(true);

        currentPhase = TentPhase.PlacingStakes;
        stakesPlaced = 0;
        phase2Progress = 0;
        phase3Progress = 0;

        if (tentSpriteRenderer != null && tentStage1_FlatOnGround != null)
            tentSpriteRenderer.sprite = tentStage1_FlatOnGround;
        
        SetupPhase1();
        UpdateInstructions();
        
        Time.timeScale = 0f;
    }

    private void SetupPhase1()
    {
        foreach (Button btn in stakeButtons)
        {
            ColorBlock colors = btn.colors;
            colors.normalColor = Color.red;
            btn.colors = colors;
            btn.interactable = true;
            
            // TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            // if (btnText != null)
            //     btnText.text = "";
        }
    }
    
    public void PlaceStake(int stakeIndex)
    {
        if (!isSetupActive || currentPhase != TentPhase.PlacingStakes) return;

        ColorBlock colors = stakeButtons[stakeIndex].colors;
        colors.normalColor = Color.green;
        stakeButtons[stakeIndex].colors = colors;
        stakeButtons[stakeIndex].interactable = false;
        
        stakesPlaced++;
        Debug.Log("Stake " + (stakeIndex + 1) + " placed! Total: " + stakesPlaced + "/" + totalStakes);

        PlaySound(stakeSound);
        UpdateInstructions();
        
        if (stakesPlaced >= totalStakes)
        {
            phaseTransitionCoroutine = StartCoroutine(TransitionToPhase2(1.5f));
        }
    }

    private IEnumerator TransitionToPhase2(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        Debug.Log("Phase 1 complete! Moving to Phase 2");
        
        if (tentSpriteRenderer != null && tentStage1_FlatOnGround != null)
            tentSpriteRenderer.sprite = tentStage1_FlatOnGround;
        
        currentPhase = TentPhase.RaisingPoles;
        SetupPhase2();
    }

    private void SetupPhase2()
    {
        for (int i = 0; i < 2 && i < stakeButtons.Length; i++)
        {
            ColorBlock colors = stakeButtons[i].colors;
            colors.normalColor = Color.blue;
            stakeButtons[i].colors = colors;
            stakeButtons[i].interactable = true;
            
            TextMeshProUGUI btnText = stakeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
                btnText.text = "Pole " + (i + 1);
        }
        
        for (int i = 2; i < stakeButtons.Length; i++)
        {
            stakeButtons[i].interactable = false;
        }
        
        UpdateInstructions();
    }

    public void RaisePole(int poleIndex)
    {
        if (!isSetupActive || currentPhase != TentPhase.RaisingPoles) return;
        
        ColorBlock colors = stakeButtons[poleIndex].colors;
        colors.normalColor = Color.cyan;
        stakeButtons[poleIndex].colors = colors;
        stakeButtons[poleIndex].interactable = false;
        
        phase2Progress++;
        Debug.Log("Pole raised! Progress: " + phase2Progress + "/2");
        
        UpdateInstructions();
        
        if (phase2Progress >= 2)
        {
            phaseTransitionCoroutine = StartCoroutine(TransitionToPhase3(1.5f));
        }
    }

    private IEnumerator TransitionToPhase3(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        Debug.Log("Phase 2 complete! Tent raised. Moving to Phase 3");
        
        if (tentSpriteRenderer != null && tentStage2_PolesRaised != null)
            tentSpriteRenderer.sprite = tentStage2_PolesRaised;
        
        currentPhase = TentPhase.FinalSetup;
        SetupPhase3();
    }

    private void SetupPhase3()
    {
        for (int i = 0; i < 2 && i < stakeButtons.Length; i++)
        {
            ColorBlock colors = stakeButtons[i].colors;
            colors.normalColor = Color.yellow;
            stakeButtons[i].colors = colors;
            stakeButtons[i].interactable = true;
            
            TextMeshProUGUI btnText = stakeButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                if (i == 0)
                    btnText.text = "Rainfly";
                else
                    btnText.text = "Guy Lines";
            }
        }
        
        UpdateInstructions();
    }

    public void CompleteFinalTask(int taskIndex)
    {
        if (!isSetupActive || currentPhase != TentPhase.FinalSetup) return;
        
        ColorBlock colors = stakeButtons[taskIndex].colors;
        colors.normalColor = Color.green;
        stakeButtons[taskIndex].colors = colors;
        stakeButtons[taskIndex].interactable = false;
        
        // TextMeshProUGUI btnText = stakeButtons[taskIndex].GetComponentInChildren<TextMeshProUGUI>();
        // if (btnText != null)
        //     btnText.text = "✓";
        
        phase3Progress++;
        Debug.Log("Final task complete! Progress: " + phase3Progress + "/2");
        
        UpdateInstructions();
        
        if (phase3Progress >= 2)
        {
            phaseTransitionCoroutine = StartCoroutine(CompleteSetup(1.5f));
        }
    }

    private IEnumerator CompleteSetup(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        PlaySound(completeSound);
        
        Debug.Log("Tent setup complete! You learned proper tent pitching!");

        if (tentSpriteRenderer != null && tentStage3_Complete != null)
            tentSpriteRenderer.sprite = tentStage3_Complete;
        
        instructionText.text = phase3Instructions[2];
        
        StartCoroutine(CloseAfterDelay(2f));
    }
    
    private void UpdateInstructions()
    {
        switch (currentPhase)
        {
            case TentPhase.PlacingStakes:
                if (stakesPlaced < phase1Instructions.Length)
                    instructionText.text = phase1Instructions[stakesPlaced];
                break;
                
            case TentPhase.RaisingPoles:
                if (phase2Progress < phase2Instructions.Length)
                    instructionText.text = phase2Instructions[phase2Progress];
                break;
                
            case TentPhase.FinalSetup:
                if (phase3Progress < phase3Instructions.Length)
                    instructionText.text = phase3Instructions[phase3Progress];
                break;
        }
    }

    private IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CloseTentSetup();
    }
    
    public void CloseTentSetup()
    {
        isSetupActive = false;
        audioSource.Stop();
        tentSetupPanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Cancel any running phase transition
        if (phaseTransitionCoroutine != null)
        {
            StopCoroutine(phaseTransitionCoroutine);
            phaseTransitionCoroutine = null;
        }
    }

    // Helper method for playing sounds
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void OnButtonClick(int buttonIndex)
    {
        switch (currentPhase)
        {
            case TentPhase.PlacingStakes:
                PlaceStake(buttonIndex);
                break;
                
            case TentPhase.RaisingPoles:
                if (buttonIndex < 2)
                    RaisePole(buttonIndex);
                break;
                
            case TentPhase.FinalSetup:
                if (buttonIndex < 2)
                    CompleteFinalTask(buttonIndex);
                break;
        }
    }

    void Update()
    {
        
    }
}