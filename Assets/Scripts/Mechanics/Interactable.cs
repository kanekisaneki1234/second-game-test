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

    [Header("UI References")]
    [SerializeField] private TMPro.TextMeshProUGUI prerequisiteText;
    [SerializeField] private GameObject tutorialPanel;
    
    private bool playerInRange = false;

    // private InteractableManager interactableManager;

    // Start is called before the first frame update
    void Start()
    {
        // if (prerequisitePopup != null)
            // prerequisitePopup.SetActive(false);

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

    // public bool GetCampfireIsLit()
    // {
    //     return interactableManager.campfireIsLit;
    // }

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
                // else
                // {
                //     ShowTentPrerequisitePopup();
                // }
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

    // private bool CanCallItADay()
    // {
    //     return interactableManager.GetTentSetup() && interactableManager.GetCampfireLit() && interactableManager.GetFoodCooked();
    // }

    private void CallItADay()
    {
        tutorialManager.NextStep();

        // tutorialPanel.SetActive(false);
        // Time.timeScale = 0f;
        // if (prerequisitePopup != null && prerequisiteText != null)
        // {

        //     prerequisiteText.text = "Tutorial Complete! You have learned:\nTent Setup\nFire Building\nOutdoor Cooking\n\nGood night, camper!";
        //     prerequisitePopup.SetActive(true);
        //     StartCoroutine(HidePopupAfterDelay(4f));
        // }
    }

    // private void ShowTentPrerequisitePopup()
    // {
    //     if (prerequisitePopup != null && prerequisiteText != null)
    //     {
    //         string missingTasks = "Complete these tasks first:\n\n";

    //         if (!interactableManager.GetTentSetup())
    //             missingTasks += "Set up your tent\n";
    //         else
    //             missingTasks += "Tent is set up\n";
                
    //         if (!interactableManager.GetCampfireLit())
    //             missingTasks += "Build and light campfire\n";
    //         else
    //             missingTasks += "Campfire is lit\n";
                
    //         if (!interactableManager.GetFoodCooked())
    //             missingTasks += "Cook your meal\n";
    //         else
    //             missingTasks += "Meal is cooked\n";

    //         prerequisiteText.text = missingTasks;
    //         prerequisitePopup.SetActive(true);
            
    //         StartCoroutine(HidePopupAfterDelay(3f));
    //     }
    // }

    // public void ShowCampfirePrerequisitesPopup(string text)
    // {
    //     if (prerequisitePopup != null && prerequisiteText != null)
    //     {
    //         prerequisiteText.text = text;
    //         prerequisitePopup.SetActive(true);
            
    //         StartCoroutine(HidePopupAfterDelay(3f));
    //     }
    // }
    // public void ShowCookingPrerequisitesPopup()
    // {
    //     if (prerequisitePopup != null && prerequisiteText != null)
    //     {
    //         string missingMaterials = "Before you start cooking, you need to pick up some mushrooms";

    //         prerequisiteText.text = missingMaterials;
    //         prerequisitePopup.SetActive(true);
            
    //         StartCoroutine(HidePopupAfterDelay(3f));
    //     }
    // }

    // private IEnumerator HidePopupAfterDelay(float delay)
    // {
    //     yield return new WaitForSecondsRealtime(delay);
    //     if (prerequisitePopup != null)
    //     {
    //         prerequisitePopup.SetActive(false);
    //         Time.timeScale = 1f;
    //     }
    // }

    public void SetTentSetup(bool isSetup)
    {
        // tentIsSetup = isSetup;
        interactableManager.SetTentSetup(isSetup);
        // UpdateTentPrompt();
        UpdatePromptBasedOnProgress();
    }

    public void SetCampfireLit(bool isLit)
    {
        // campfireIsLit = isLit;
        interactableManager.SetCampfireLit(isLit);
        // UpdateCampfirePrompt();
        UpdatePromptBasedOnProgress();
    }

    public void SetFoodCooked(bool isCooked)
    {
        // foodIsCooked = isCooked;
        interactableManager.SetFoodCooked(isCooked);
        // UpdateTentPrompt();
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

    // public void UpdateTentPrompt()
    // {
    //     if (isTent)
    //     {
    //         if (!interactableManager.GetTentSetup())
    //         {
    //             interactionPrompt = "Press E to set up tent";
    //         }
    //         else
    //         {
    //             interactionPrompt = "Press E to call it a day";
    //         }
            
    //         if (playerInRange && InteractionPromptManager.Instance != null)
    //         {
    //             InteractionPromptManager.Instance.ShowPrompt(interactionPrompt);
    //         }
    //     }
    // }

    // private void UpdateCampfirePrompt()
    // {
    //     if (isCampfire)
    //     {
    //         if (interactableManager.GetCampfireLit())
    //         {
    //             interactionPrompt = "Press E to cook";
    //         }
    //         else
    //         {
    //             interactionPrompt = "Press E to build fire";
    //         }
            
    //         if (playerInRange && InteractionPromptManager.Instance != null)
    //         {
    //             InteractionPromptManager.Instance.ShowPrompt(interactionPrompt);
    //         }
    //     }
    // }

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
