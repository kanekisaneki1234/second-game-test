using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampfireManager : MonoBehaviour
{
    [Header("Interactable Reference")]
    [SerializeField] private Interactable campfireInteractable;

    [Header("UI References")]
    [SerializeField] private GameObject campfirePanel;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button placeTinderButton;
    [SerializeField] private Button placeKindlingButton;
    [SerializeField] private Button placeLogsButton;
    [SerializeField] private Button lightFireButton;
    
    [Header("Slot Images")]
    [SerializeField] private Image tinderSlot;
    [SerializeField] private Image kindlingSlot;
    [SerializeField] private Image logSlot;
    
    [Header("Game References")]
    [SerializeField] private FireResourceManager resourceManager;
    [SerializeField] private GameObject campfireObject;
    [SerializeField] private SpriteRenderer campfireSpriteRenderer;

    [Header("Campfire Stage Sprites")]
    [SerializeField] private Sprite campfireStage0_Empty;
    [SerializeField] private Sprite campfireStage1_TinderPlaced;
    [SerializeField] private Sprite campfireStage2_MaterialsArranged;
    [SerializeField] private Sprite campfireStage3_Burning;

    [Header("Fire Effects")]
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private GameObject smokeEffect;
    
    private bool tinderPlaced = false;
    private bool kindlingPlaced = false;
    private bool logsPlaced = false;
    
    [Header("Educational Tips")]
    [SerializeField] private string[] tips = new string[]
    {
        "Tip: Tinder catches fire easily - dry grass, paper, or bark shavings work best!",
        "Tip: Kindling is small sticks that catch from tinder and burn longer.",
        "Tip: Logs are your main fuel - add them last for a long-lasting fire.",
        "Remember: Always clear a 10-foot area around your fire and have water nearby!"
    };

    // Coroutine reference for cancellation if needed
    private Coroutine closePanelCoroutine;

    void Start()
    {
        if (campfirePanel != null)
            campfirePanel.SetActive(false);

        if (campfireSpriteRenderer != null && campfireStage0_Empty != null)
            campfireSpriteRenderer.sprite = campfireStage0_Empty;
            
        if (fireParticles != null)
            fireParticles.Stop();
            
        if (smokeEffect != null)
            smokeEffect.SetActive(false);
    }

    public void StartCampfireBuilding()
    {
        if (resourceManager != null)
        {
            resourceManager.ShowInventory();
        }

        if (!resourceManager.HasAllResources())
        {
            Debug.Log("You need to collect more materials first!");

            string missingItems = "Still need:\n";
            if (!resourceManager.HasEnoughTinder())
                missingItems += "- More tinder\n";
            if (!resourceManager.HasEnoughKindling())
                missingItems += "- More kindling\n";
            if (!resourceManager.HasEnoughLogs())
                missingItems += "- More logs\n";

            instructionText.text = "Collect all materials first!\n\n" + missingItems;
            return;
        }
        
        campfirePanel.SetActive(true);
        Time.timeScale = 0f;
        
        ResetFireBuilding();
        UpdateInstructions();
    }

    private void ResetFireBuilding()
    {
        tinderPlaced = false;
        kindlingPlaced = false;
        logsPlaced = false;

        if (campfireSpriteRenderer != null && campfireStage0_Empty != null)
            campfireSpriteRenderer.sprite = campfireStage0_Empty;
        
        tinderSlot.color = new Color(1f, 1f, 0.7f, 0.5f);
        kindlingSlot.color = new Color(0.8f, 0.6f, 0.4f, 0.5f);
        logSlot.color = new Color(0.6f, 0.4f, 0.2f, 0.5f);
        
        placeTinderButton.interactable = true;
        placeKindlingButton.interactable = false;
        placeLogsButton.interactable = false;
        lightFireButton.interactable = false;
    }

    public void PlaceTinder()
    {
        tinderPlaced = true;
        tinderSlot.color = Color.yellow;
        placeTinderButton.interactable = false;
        placeKindlingButton.interactable = true;
        
        Debug.Log("Tinder placed!");

        if (campfireSpriteRenderer != null && campfireStage1_TinderPlaced != null)
        {
            campfireSpriteRenderer.sprite = campfireStage1_TinderPlaced;
        }

        UpdateInstructions();
    }

    public void PlaceKindling()
    {
        if (!tinderPlaced)
        {
            instructionText.text = "Place tinder first! It goes on the bottom.";
            return;
        }
        
        kindlingPlaced = true;
        kindlingSlot.color = new Color(0.8f, 0.5f, 0.2f);
        placeKindlingButton.interactable = false;
        placeLogsButton.interactable = true;
        
        Debug.Log("Kindling placed!");
        UpdateInstructions();
    }

    public void PlaceLogs()
    {
        if (!kindlingPlaced)
        {
            instructionText.text = "Place kindling second! Build from small to large.";
            return;
        }
        
        logsPlaced = true;
        logSlot.color = new Color(0.5f, 0.3f, 0.1f);
        placeLogsButton.interactable = false;
        lightFireButton.interactable = true;
        
        if (campfireSpriteRenderer != null && campfireStage2_MaterialsArranged != null)
        {
            campfireSpriteRenderer.sprite = campfireStage2_MaterialsArranged;
        }

        Debug.Log("Logs placed!");
        UpdateInstructions();
    }

    public void LightFire()
    {
        if (!tinderPlaced || !kindlingPlaced || !logsPlaced)
        {
            instructionText.text = "Arrange all materials first!";
            return;
        }
        
        Debug.Log("Fire lit successfully!");
        instructionText.text = "Fire lit successfully!\n" + tips[3];

        if (campfireSpriteRenderer != null && campfireStage3_Burning != null)
        {
            campfireSpriteRenderer.sprite = campfireStage3_Burning;
        }

        if (fireParticles != null)
        {
            ParticleSystem fireParticles = campfireObject.GetComponentInChildren<ParticleSystem>();
            if (fireParticles != null)
                fireParticles.Play();
        }
        
        if (smokeEffect != null)
        {
            smokeEffect.SetActive(true);
        }
        
        resourceManager.UseResources();

        if (campfireInteractable != null)
        {
            campfireInteractable.SetCampfireLit(true);
        }
        
        closePanelCoroutine = StartCoroutine(CloseAfterDelay(3f));
    }

    private IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CloseCampfirePanel();
    }

    private void UpdateInstructions()
    {
        if (!tinderPlaced)
            instructionText.text = tips[0];
        else if (!kindlingPlaced)
            instructionText.text = tips[1];
        else if (!logsPlaced)
            instructionText.text = tips[2];
        else
            instructionText.text = "Ready to light! Click 'Light Fire' button.";
    }
    
    public void CloseCampfirePanel()
    {
        campfirePanel.SetActive(false);
        Time.timeScale = 1f;
        
        // Cancel any running close coroutine
        if (closePanelCoroutine != null)
        {
            StopCoroutine(closePanelCoroutine);
            closePanelCoroutine = null;
        }
    }

    void Update()
    {
        
    }
}