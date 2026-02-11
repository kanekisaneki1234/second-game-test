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
    [SerializeField] private int resourceAmount = 1; 
    
    [Header("Visual Settings")]
    [SerializeField] private GameObject visualObject;
    
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
        
        resourceManager.AddResource(resourceType, resourceAmount);
        
        alreadyCollected = true;
        
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        Debug.Log($"Collected {resourceAmount}x {resourceType.ToString()}");
        
        ShowCollectionFeedback();
        
        if (visualObject != null)
        {
            visualObject.SetActive(false);
        }

        if (InteractionPromptManager.Instance == null)
        {
            InteractionPromptManager.Instance.HidePrompt();
        }
    }

    private void ShowCollectionFeedback()
    {
        string message = $"+{resourceAmount} {resourceType}";
        Debug.Log(message);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !alreadyCollected)
        {
            playerInRange = true;
            
            string resourceName = GetResourceDisplayName();
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
