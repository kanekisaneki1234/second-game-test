using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private TentSetupManager tentSetupManager;
    [SerializeField] private CampfireManager campfireManager;
    [SerializeField] private CookingManager cookingManager;
    [SerializeField] private InteractableManager interactableManager;
    [SerializeField] private TutorialManager tutorialManager;

    [Header("Interaction Type")]
    [SerializeField] private bool isTent = false;
    [SerializeField] private bool isCampfire = false;
    
    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePromptBasedOnProgress();
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
        
        if (tentSetupManager != null && isTent)
        {
            if (!interactableManager.GetTentSetup())
            {
                tentSetupManager.StartTentSetup();                
            }
            else
            {
                if (interactableManager.CanCallItADay())
                {
                    CallItADay();
                }
            }
        }

        if (campfireManager != null && isCampfire)
        {
            if (!interactableManager.GetCampfireLit())
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

    private void CallItADay()
    {
        tutorialManager.NextStep();
    }

    public void SetTentSetup(bool isSetup)
    {
        interactableManager.SetTentSetup(isSetup);
        UpdatePromptBasedOnProgress();
    }

    public void SetCampfireLit(bool isLit)
    {
        interactableManager.SetCampfireLit(isLit);
        UpdatePromptBasedOnProgress();
    }

    public void SetFoodCooked(bool isCooked)
    {
        interactableManager.SetFoodCooked(isCooked);
        UpdatePromptBasedOnProgress();
    }

    public bool GetCampfireIsLit()
    {
        return interactableManager.GetCampfireLit();
    }

    private void UpdatePromptBasedOnProgress()
    {
        if (isTent)
        {
            if (!interactableManager.GetTentSetup())
            {
                interactionPrompt = "Press E to set up tent";
            }
            else
            {
                interactionPrompt = "Press E to call it a day";
            }
        }
        else if (isCampfire)
        {
            if (!interactableManager.GetCampfireLit())
            {
                interactionPrompt = "Press E to build fire";
            }
            else
            {
                interactionPrompt = "Press E to cook";
            }
        }
        
        if (playerInRange && InteractionPromptManager.Instance != null)
        {
            InteractionPromptManager.Instance.ShowPrompt(interactionPrompt);
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
