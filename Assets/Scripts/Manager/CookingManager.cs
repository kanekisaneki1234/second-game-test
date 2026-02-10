using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject cookingInventoryPanel;
    [SerializeField] private GameObject cookingPanel;
    [SerializeField] private TextMeshProUGUI mushroomText;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private Button placeMushroomsButton;
    [SerializeField] private Button cookButton;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private SpriteRenderer campfireSpriteRenderer;
    
    [Header("Visual References")]
    [SerializeField] private Image panSlot;
    [SerializeField] private Sprite emptyPanSprite;
    [SerializeField] private Sprite mushroomsInPanSprite;
    [SerializeField] private Sprite cookedMealSprite;
    
    [Header("Food Inventory")]
    [SerializeField] private int mushroomsNeeded = 5;
    private int mushroomCount = 0;
    
    [Header("Campfire Reference")]
    [SerializeField] private CampfireManager campfireManager;

    [Header("Interactable Reference")]
    [SerializeField] private Interactable campfireInteractable;

    [Header("Shared Sprite Data")]
    [SerializeField] private CampfireSprites campfireSprites;
    
    [Header("Cooking State")]
    private bool mushroomsPlaced = false;
    private bool mealCooked = false;
    
    [Header("Educational Tips")]
    [SerializeField] private string[] cookingTips = new string[]
    {
        "Tip: Only forage mushrooms you can positively identify!",
        "Tip: Always cook wild food thoroughly to kill bacteria.",
        "Tip: Never leave food or cookware unattended - it attracts wildlife!",
        "Meal ready! Remember to clean your cookware and pack out all waste."
    };

    private Coroutine closePanelCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {
        if (cookingInventoryPanel != null)
            cookingInventoryPanel.SetActive(false);
            
        if (cookingPanel != null)
            cookingPanel.SetActive(false);
            
        UpdateInventoryUI();
    }

    public void AddFood(CollectibleFood.FoodType foodType, int amount = 1)
    {
        switch (foodType)
        {
            case CollectibleFood.FoodType.Mushroom or CollectibleFood.FoodType.MushroomGrove:
                mushroomCount = Mathf.Min(mushroomCount + amount, mushroomsNeeded);
                break;
            // case CollectibleFood.FoodType.MushroomGrove:
            //     mushroomCount = Mathf.Min(mushroomCount + amount, mushroomsNeeded);
            //     break;
        }
        
        UpdateInventoryUI();
        
        // Show inventory panel when first food collected // DISCARDED because I chose to show the inv after interacting with campfire
        // if (cookingInventoryPanel != null && !cookingInventoryPanel.activeSelf)
        // {
        //     cookingInventoryPanel.SetActive(true);
        // }
    }

    public bool HasEnoughFood()
    {
        return mushroomCount >= mushroomsNeeded;
    }

    public void StartCooking()
    {
        // if (campfireManager != null && campfireInteractable != null && !campfireInteractable.GetCampfireIsLit())
        // {
        //     Debug.Log("You need to light the campfire first!");
        //     instructionText.text = "Light the campfire before cooking!";
        //     return;
        // }

        if (campfireSpriteRenderer != null && campfireSprites != null)
        {
            campfireSpriteRenderer.sprite = campfireSprites.stage4_Cooking;
        }

        if (campfireManager != null && campfireInteractable != null && campfireInteractable.GetCampfireIsLit() && !HasEnoughFood())
        {
            cookingInventoryPanel.SetActive(true);
            cookingPanel.SetActive(true);
            // tutorialPanel.SetActive(false);

            Debug.Log("Collect more mushrooms first!");
            instructionText.text = "You need to collect more mushrooms!\n\n" +
                                  $"Current: {mushroomCount}/{mushroomsNeeded}";
            // campfireInteractable.ShowCookingPrerequisitesPopup();
            return;
        }
        
        cookingPanel.SetActive(true);
        tutorialPanel.SetActive(false);
        Time.timeScale = 0f;
        
        ResetCooking();
        UpdateCookingInstructions();
    }

    private void ResetCooking()
    {
        mushroomsPlaced = false;
        mealCooked = false;
        
        if (panSlot != null && emptyPanSprite != null)
            panSlot.sprite = emptyPanSprite;
        
        placeMushroomsButton.interactable = true;
        cookButton.interactable = false;
    }

    public void PlaceMushrooms()
    {
        if (!HasEnoughFood())
        {
            instructionText.text = "Not enough mushrooms!";
            return;
        }
        
        mushroomsPlaced = true;
        
        if (panSlot != null && mushroomsInPanSprite != null)
        {
            Debug.Log("Pan in Sprite");
            panSlot.sprite = mushroomsInPanSprite;
        }
        
        placeMushroomsButton.interactable = false;
        cookButton.interactable = true;
        
        Debug.Log("Mushrooms placed in pan!");
        UpdateCookingInstructions();
    }

    public void CookMeal()
    {
        if (!mushroomsPlaced)
        {
            instructionText.text = "Place mushrooms in the pan first!";
            return;
        }
        
        mealCooked = true;
        
        if (panSlot != null && cookedMealSprite != null)
            panSlot.sprite = cookedMealSprite;
        
        cookButton.interactable = false;
        
        mushroomCount = 0;
        UpdateInventoryUI();
        
        Debug.Log("Meal cooked successfully!");
        instructionText.text = "Meal cooked!\n" + cookingTips[3];
        
        if (campfireSpriteRenderer != null && campfireSprites != null)
        {
            campfireSpriteRenderer.sprite = campfireSprites.stage5_MealReady;
        }

        closePanelCoroutine = StartCoroutine(CloseAfterDelay(3f));
    }

    private IEnumerator CloseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        CloseCookingPanel();
    }

    private void UpdateCookingInstructions()
    {
        if (!mushroomsPlaced)
            instructionText.text = cookingTips[0] + "\n\n" + cookingTips[1];
        else if (!mealCooked)
            instructionText.text = cookingTips[2] + "\n\nClick 'Cook Food' to prepare your meal!";
    }

    private void UpdateInventoryUI()
    {
        if (mushroomText != null)
        {
            mushroomText.text = $"Mushrooms: {mushroomCount}/{mushroomsNeeded}";
            mushroomText.color = mushroomCount >= mushroomsNeeded ? Color.green : Color.white;
        }
    }
    
    public void CloseCookingPanel()
    {
        if (campfireInteractable != null)
        {
            campfireInteractable.SetFoodCooked(true);
            // campfireSpriteRenderer.sprite = cookedMealSprite;
        }

        cookingPanel.SetActive(false);
        cookingInventoryPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        Time.timeScale = 1f;
    }

    public int GetMushroomCount() { return mushroomCount; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
