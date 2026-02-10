using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireResourceManager : MonoBehaviour
{
    [Header("Interactable Manager Reference")]
    [SerializeField] private InteractableManager interactableManager;

    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI tinderText;
    [SerializeField] private TextMeshProUGUI kindlingText;
    [SerializeField] private TextMeshProUGUI logsText;

    [Header("Resource Requirements")]
    [SerializeField] private int tinderNeeded = 3;
    [SerializeField] private int kindlingNeeded = 3;
    [SerializeField] private int logsNeeded = 3;

    private int tinderCount = 0;
    private int kindlingCount = 0;
    private int logsCount = 0;

    private bool inventoryRevealed = false;
    void Start()
    {
        UpdateInventoryUI();

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    public void ShowInventory()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            inventoryRevealed = true;
        }
    }

    public void AddResource(InteractableResource.ResourceType type, int amount = 1)
    {
        switch (type)
        {
            case InteractableResource.ResourceType.Tinder:
                tinderCount = Mathf.Min(tinderCount + amount, tinderNeeded);
                break;
            case InteractableResource.ResourceType.Kindling:
                kindlingCount = Mathf.Min(kindlingCount + amount, kindlingNeeded);
                break;
            case InteractableResource.ResourceType.Logs:
                logsCount = Mathf.Min(logsCount + amount, logsNeeded);
                break;
        }

        if (HasAllResources())
        {
            DisableResourcesByTag("CampfireResource");
        }
        
        UpdateInventoryUI();
        
        // Show inventory when first item collected
        if (!inventoryRevealed)
        {
            ShowInventory();
        }
    }

    public bool HasAllResources()
    {
        return tinderCount >= tinderNeeded && 
               kindlingCount >= kindlingNeeded && 
               logsCount >= logsNeeded;
    }

    public bool HasEnoughTinder() { return tinderCount >= tinderNeeded; }
    public bool HasEnoughKindling() { return kindlingCount >= kindlingNeeded; }
    public bool HasEnoughLogs() { return logsCount >= logsNeeded; }
    
    public void UseResources()
    {
        tinderCount = 0;
        kindlingCount = 0;
        logsCount = 0;
        UpdateInventoryUI();
        inventoryPanel.SetActive(false);
    }

    private void UpdateInventoryUI()
    {
        if (tinderText != null)
        {
            tinderText.text = $"Tinder: {tinderCount}/{tinderNeeded}";
            // Change color if requirement met
            tinderText.color = tinderCount >= tinderNeeded ? Color.green : Color.white;
        }
        
        if (kindlingText != null)
        {
            kindlingText.text = $"Kindling: {kindlingCount}/{kindlingNeeded}";
            kindlingText.color = kindlingCount >= kindlingNeeded ? Color.green : Color.white;
        }
        
        if (logsText != null)
        {
            logsText.text = $"Logs: {logsCount}/{logsNeeded}";
            logsText.color = logsCount >= logsNeeded ? Color.green : Color.white;
        }
    }

    private void DisableResourcesByTag(string tag)
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
        
        foreach (GameObject resource in resources)
        {
            Collider2D collider = resource.GetComponent<Collider2D>();
            
            if (collider != null)
            {
                collider.isTrigger = false;
                Debug.Log($"Disabled collider for {resource.name} - inventory full!");
            }
        }
    }

    public int GetTinderCount() { return tinderCount; }
    public int GetKindlingCount() { return kindlingCount; }
    public int GetLogsCount() { return logsCount; }

    // Update is called once per frame
    void Update()
    {

    }
}
