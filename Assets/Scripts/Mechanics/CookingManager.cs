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
    
    [Header("Visual References")]
    [SerializeField] private Image panSlot;
    [SerializeField] private Sprite emptyPanSprite;
    [SerializeField] private Sprite mushroomsInPanSprite;
    [SerializeField] private Sprite cookedMealSprite;
    
    [Header("Food Inventory")]
    [SerializeField] private int mushroomsNeeded = 3;
    private int mushroomCount = 0;
    
    [Header("Campfire Reference")]
    [SerializeField] private CampfireManager campfireManager;
    
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
            case CollectibleFood.FoodType.Mushrooms:
                mushroomCount = Mathf.Min(mushroomCount + amount, mushroomsNeeded);
                break;
        }
        
        UpdateInventoryUI();
        
        // Show inventory panel when first food collected
        if (cookingInventoryPanel != null && !cookingInventoryPanel.activeSelf)
        {
            cookingInventoryPanel.SetActive(true);
        }
    }

    public bool HasEnoughFood()
    {
        return mushroomCount >= mushroomsNeeded;
    }

    public void StartCooking()
    {
        // Check if campfire is lit
        // You'll need to add a public method in CampfireManager to check this
        
        // Check if player has food
        if (!HasEnoughFood())
        {
            Debug.Log("Collect more mushrooms first!");
            instructionText.text = "You need to collect more mushrooms!\n\n" +
                                  $"Current: {mushroomCount}/{mushroomsNeeded}";
            return;
        }
        
        // Open cooking panel
        cookingPanel.SetActive(true);
        Time.timeScale = 0f;
        
        ResetCooking();
        UpdateCookingInstructions();
    }

    private void ResetCooking()
    {
        mushroomsPlaced = false;
        mealCooked = false;
        
        // Set pan to empty
        if (panSlot != null && emptyPanSprite != null)
            panSlot.sprite = emptyPanSprite;
        
        // Enable place button
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
        
        // Change pan sprite to show mushrooms
        if (panSlot != null && mushroomsInPanSprite != null)
            panSlot.sprite = mushroomsInPanSprite;
        
        // Disable place button, enable cook button
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
        
        // Change to cooked meal sprite
        if (panSlot != null && cookedMealSprite != null)
            panSlot.sprite = cookedMealSprite;
        
        cookButton.interactable = false;
        
        // Use up the mushrooms
        mushroomCount = 0;
        UpdateInventoryUI();
        
        Debug.Log("Meal cooked successfully!");
        instructionText.text = "Meal cooked!\n" + cookingTips[3];
        
        // Invoke("CloseCookingPanel", 3f);
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
        cookingPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public int GetMushroomCount() { return mushroomCount; }

    // Update is called once per frame
    void Update()
    {
        
    }
}
