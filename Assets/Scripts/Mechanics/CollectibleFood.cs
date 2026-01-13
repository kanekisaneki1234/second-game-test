using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFood : MonoBehaviour
{
    public enum FoodType
    {
        Mushrooms
    }

    [Header("Food Settings")]
    [SerializeField] private FoodType foodType = FoodType.Mushrooms;
    [SerializeField] private int foodAmount = 1;
    
    [Header("Visual Settings")]
    [SerializeField] private GameObject visualObject;
    [SerializeField] private string interactionPrompt = "Press E to collect";
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip collectSound;
    
    private bool playerInRange = false;
    private bool alreadyCollected = false;
    private CookingManager cookingManager;
    // Start is called before the first frame update
    void Start()
    {
        cookingManager = FindObjectOfType<CookingManager>();
        
        if (cookingManager == null)
        {
            Debug.LogError("CookingManager not found in scene!");
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
            CollectFood();
        }
    }

    private void CollectFood()
    {
        if (cookingManager == null || alreadyCollected) return;
        
        // Add food to inventory
        cookingManager.AddFood(foodType, foodAmount);
        
        alreadyCollected = true;
        
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        
        Debug.Log($"Collected {foodAmount}x {foodType.ToString()}");
        
        if (visualObject != null)
                visualObject.SetActive(false);
        
        // Hide prompt
        if (InteractionPromptManager.Instance != null)
        {
            InteractionPromptManager.Instance.HidePrompt();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !alreadyCollected)
        {
            playerInRange = true;
            
            string foodName = GetFoodDisplayName();
            
            if (InteractionPromptManager.Instance != null)
            {
                InteractionPromptManager.Instance.ShowPrompt($"Press E to collect {foodName}");
            }
            else
            {
                Debug.Log($"{interactionPrompt} {foodName}");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            if (InteractionPromptManager.Instance != null)
            {
                InteractionPromptManager.Instance.HidePrompt();
            }
        }
    }

    private string GetFoodDisplayName()
    {
        switch (foodType)
        {
            case FoodType.Mushrooms:
                return "Wild Mushrooms";
            default:
                return foodType.ToString();
        }
    }
}
