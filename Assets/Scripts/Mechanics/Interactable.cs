using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private TentSetupManager tentSetupManager;
    [SerializeField] private CampfireManager campfireManager;
    [SerializeField] private CookingManager cookingManager;

    [Header("Interaction Type")]
    [SerializeField] private bool isCampfire = false;
    [SerializeField] private bool campfireIsLit = false;
    
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
        
        if (tentSetupManager != null)
        {
            tentSetupManager.StartTentSetup();
        }

        if (campfireManager != null && isCampfire)
        {
            if (!campfireIsLit)
            {
                campfireManager.StartCampfireBuilding();
            }
            else
            {
                if (cookingManager != null)
                {
                    cookingManager.StartCooking();
                }
            }
        }
    }

    public void SetCampfireLit(bool isLit)
    {
        campfireIsLit = isLit;
        
        // Update interaction prompt
        if (isLit)
        {
            interactionPrompt = "Press E to cook";
        }
        else
        {
            interactionPrompt = "Press E to build fire";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press E to " + interactionPrompt);

            if (InteractionPromptManager.Instance != null)
            {
                InteractionPromptManager.Instance.ShowPrompt(interactionPrompt);
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
}
