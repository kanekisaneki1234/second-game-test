using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableResource : MonoBehaviour
{
    public enum ResourceType
    {
        Tinder,
        Kindling,
        Logs
    }

    [Header("Resource Settings")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int resourceAmount = 1; // How many you get per collection
    
    [Header("Visual Settings")]
    [SerializeField] private GameObject visualObject;
    // [SerializeField] private string interactionPrompt = "Press E to collect";
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip collectSound;
    
    private bool playerInRange = false;
    private bool alreadyCollected = false;
    private FireResourceManager resourceManager;
    // Start is called before the first frame update
    void Start()
    {
        resourceManager = FindObjectOfType<FireResourceManager>();
        
        if (resourceManager == null)
        {
            Debug.LogError("FireResourceManager not found in scene!");
        }
        
        // If no visual object specified, use this GameObject's sprite
        if (visualObject == null)
        {
            visualObject = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !alreadyCollected && Input.GetKeyDown(KeyCode.E))
        {
            CollectResource();
        }
    }

    private void CollectResource()
    {
        if (resourceManager == null || alreadyCollected) return;
        
        // Add resource to inventory
        resourceManager.AddResource(resourceType, resourceAmount);
        
        // Mark as collected
        alreadyCollected = true;
        
        // Play sound if available
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        // Visual feedback
        Debug.Log($"Collected {resourceAmount}x {resourceType.ToString()}");
        
        // Show collection message on screen (optional)
        ShowCollectionFeedback();
        
        // Disable/hide the visual object
        if (visualObject != null)
        {
            visualObject.SetActive(false);
        }

        // InteractionPromptManager.Instance?.HidePrompt();
        if (InteractionPromptManager.Instance == null)
        {
            InteractionPromptManager.Instance.HidePrompt();
        }
        // if (myObject != null)
        // {
        //     myObject.DoSomething();
        // }
        
        // Optionally destroy after a delay (in case we want to respawn later)
        // Destroy(gameObject, 0.5f);
    }

    private void ShowCollectionFeedback()
    {
        // Create floating text showing what was collected
        // This is optional - you can implement this later
        string message = $"+{resourceAmount} {resourceType}";
        Debug.Log(message);
        
        // TODO: Create floating text UI element (optional polish)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !alreadyCollected)
        {
            playerInRange = true;
            
            // Show prompt
            string resourceName = GetResourceDisplayName();
            // Debug.Log($"{interactionPrompt} {resourceName}");
            InteractionPromptManager.Instance?.ShowPrompt($"Press E to collect {resourceName}");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            InteractionPromptManager.Instance?.HidePrompt();
        }
    }
    
    private string GetResourceDisplayName()
    {
        switch (resourceType)
        {
            case ResourceType.Tinder:
                return "Dry Grass/Tinder";
            case ResourceType.Kindling:
                return "Small Sticks/Kindling";
            case ResourceType.Logs:
                return "Logs/Firewood";
            default:
                return resourceType.ToString();
        }
    }
}
