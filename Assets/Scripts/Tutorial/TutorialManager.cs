using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialTitle;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private TextMeshProUGUI stepCounter;
    
    [Header("Interactive Objects")]
    [SerializeField] private GameObject tentObject;
    [SerializeField] private GameObject campfireObject;
    [SerializeField] private List<GameObject> tinderObjects;
    [SerializeField] private List<GameObject> kindlingObjects;
    [SerializeField] private List<GameObject> logObjects;
    [SerializeField] private List<GameObject> mushroomObjects;
    
    [Header("Managers")]
    [SerializeField] private TentSetupManager tentSetupManager;
    [SerializeField] private FireResourceManager fireResourceManager;
    [SerializeField] private CampfireManager campfireManager;
    [SerializeField] private CookingManager cookingManager;
    [SerializeField] private InteractableManager interactableManager;
    
    [Header("Player")]
    [SerializeField] private GameObject player;
    
    private int currentStep = 1;
    private bool tutorialActive = false;
    private bool tutorialComplete = false;
    
    private enum TutorialStep
    {
        Welcome,
        GoToTent,
        SetupTent,
        GoToCampfire,
        CollectTinder,
        CollectKindling,
        CollectLogs,
        GoBackToCampfire,
        BuildFire,
        CollectMushrooms,
        CookMeal,
        Complete
    }
    
    private TutorialStep[] tutorialSteps = new TutorialStep[]
    {
        TutorialStep.Welcome,
        TutorialStep.GoToTent,
        TutorialStep.SetupTent,
        TutorialStep.GoToCampfire,
        TutorialStep.CollectTinder,
        TutorialStep.CollectKindling,
        TutorialStep.CollectLogs,
        TutorialStep.GoBackToCampfire,
        TutorialStep.BuildFire,
        TutorialStep.CollectMushrooms,
        TutorialStep.CookMeal,
        TutorialStep.Complete
    };
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialActive || tutorialComplete) return;
        
        CheckStepCompletion();
    }

    public void StartTutorial()
    {
        Debug.Log("The Tutorial has started! (on paper)");
        tutorialActive = true;
        currentStep = 0;
        
        if (tutorialPanel != null)
        {
            Debug.Log("Works!");
            tutorialPanel.SetActive(true);
        }
        
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        if (currentStep >= tutorialSteps.Length)
        {
            CompleteTutorial();
            return;
        }
        
        TutorialStep step = tutorialSteps[currentStep];
        
        ClearAllHighlights();
        
        switch (step)
        {
            case TutorialStep.Welcome:
                ShowWelcome();
                break;
            case TutorialStep.GoToTent:
                ShowGoToTent();
                break;
            case TutorialStep.SetupTent:
                ShowSetupTent();
                break;
            case TutorialStep.GoToCampfire:
                ShowGoToCampfire();
                break;
            case TutorialStep.CollectTinder:
                ShowCollectResources("tinder");
                break;
            case TutorialStep.CollectKindling:
                ShowCollectResources("kindling");
                break;
            case TutorialStep.CollectLogs:
                ShowCollectResources("logs");
                break;
            case TutorialStep.GoBackToCampfire:
                ShowGoBackToCampfire();
                break;
            case TutorialStep.BuildFire:
                ShowBuildFire();
                break;
            case TutorialStep.CollectMushrooms:
                ShowCollectMushrooms();
                break;
            case TutorialStep.CookMeal:
                ShowCookMeal();
                break;
        }
    }

    private void ShowWelcome()
    {
        tutorialTitle.text = "Welcome to Camping Simulator!";
        tutorialText.text = "Learn the essentials of camping:\n\n" +
                           "• Set up your tent properly\n" +
                           "• Build and light a campfire safely\n" +
                           "• Forage and cook food\n" +
                           "• Practice Leave No Trace principles\n\n" +
                           "Use WASD to move and E to interact.\n\n" +
                           "Ready to start? Let's set up camp!";
        
        StartCoroutine(AutoAdvanceAfterDelay(5f));
    }

    private void ShowGoToTent()
    {
        tutorialTitle.text = "Step 1: Find Your Tent";
        tutorialText.text = "First, let's set up shelter!\n\n" +
                           "Walk to the highlighted tent location.\n\n" +
                           "Tip: Always set up your tent on flat ground, away from water and hazards.";
        
        HighlightObject(tentObject);
    }

    private void ShowSetupTent()
    {
        tutorialTitle.text = "Step 2: Set Up Your Tent";
        tutorialText.text = "Press E near the tent to start setup.\n\n" +
                           "You'll learn how to:\n" +
                           "• Place stakes properly\n" +
                           "• Raise tent poles\n" +
                           "• Secure the rainfly\n\n" +
                           "Follow the on-screen instructions!";
        
        HighlightObject(tentObject);
    }

    private void ShowGoToCampfire()
    {
        tutorialTitle.text = "Step 3: Find a Spot for the Campfire Site";
        tutorialText.text = "Great! Now we need to set up a campfire.\n\n" +
                           "Walk to the highlighted campfire site.\n\n" +
                           "Tip: Build fires in clear areas, away from vegetation and at a safe distance from the tent. A place close to a water body would be perfect.";
        
        HighlightObject(campfireObject);
    }

    private void ShowCollectResources(string resourceType)
    {
        List<GameObject> resources = null;
        // string displayName = "";
        
        switch (resourceType)
        {
            case "tinder":
                resources = tinderObjects;
                tutorialTitle.text = "Step 4: Collect Tinder";
                tutorialText.text = "Collect 3 pieces of tinder for your fire.\n\n" +
                                   "Tinder catches fire easily - look for:\n" +
                                   "• Dry grass\n" +
                                   "• Dead leaves\n" +
                                   "• Small bark pieces\n\n" +
                                   "Walk to highlighted items and press E to collect.";
                break;
            case "kindling":
                resources = kindlingObjects;
                tutorialTitle.text = "Step 5: Collect Kindling";
                tutorialText.text = "Collect 3 pieces of kindling.\n\n" +
                                   "Kindling burns longer than tinder:\n" +
                                   "• Small dry sticks\n" +
                                   "• Twigs (pencil-thick)\n\n" +
                                   "These help transition from tinder to logs.";
                break;
            case "logs":
                resources = logObjects;
                tutorialTitle.text = "Step 6: Collect Logs";
                tutorialText.text = "Collect 3 logs for your fire.\n\n" +
                                   "Logs are your main fuel:\n" +
                                   "• Wrist-thick branches\n" +
                                   "• Split wood\n\n" +
                                   "These keep your fire burning long-term.";
                break;
        }
        
        if (resources != null)
        {
            foreach (GameObject resource in resources)
            {
                if (resource != null && resource.activeInHierarchy)
                {
                    HighlightObject(resource);
                }
            }
        }
    }

    private void ShowGoBackToCampfire()
    {
        tutorialTitle.text = "Step 7: Go to Campfire Site";
        tutorialText.text = "Great! You've collected all fire materials.\n\n" +
                           "Now walk to the highlighted campfire site.\n\n" +
                           "Tip: Build fires in clear areas, away from vegetation and at a safe distance from the tent.";
        
        HighlightObject(campfireObject);
    }

    private void ShowBuildFire()
    {
        tutorialTitle.text = "Step 8: Build Your Campfire";
        tutorialText.text = "Press E at the campfire to start building.\n\n" +
                           "You'll learn the proper fire lay:\n" +
                           "1. Tinder on bottom (catches easily)\n" +
                           "2. Kindling in middle (burns longer)\n" +
                           "3. Logs on top (main fuel)\n\n" +
                           "Follow the instructions to build safely!";
        
        HighlightObject(campfireObject);
    }

    private void ShowCollectMushrooms()
    {
        tutorialTitle.text = "Step 9: Forage for Food";
        tutorialText.text = "Collect 3 mushrooms for cooking.\n\n" +
                           "Important Safety:\n" +
                           "• Only forage mushrooms you can POSITIVELY identify\n" +
                           "• When in doubt, leave it out!\n" +
                           "• Some mushrooms are poisonous\n\n" +
                           "• Find the highlighted mushrooms.";
        
        foreach (GameObject mushroom in mushroomObjects)
        {
            if (mushroom != null && mushroom.activeInHierarchy)
            {
                HighlightObject(mushroom);
            }
        }
    }

    private void ShowCookMeal()
    {
        tutorialTitle.text = "Step 10: Cook Your Meal";
        tutorialText.text = "Return to the lit campfire and press E to cook.\n\n" +
                           "You'll learn about:\n" +
                           "• Cooking food thoroughly\n" +
                           "• Food safety in the backcountry\n" +
                           "• Cleaning up to avoid attracting wildlife\n\n" +
                           "Press E at the campfire to start cooking!";
        
        HighlightObject(campfireObject);
    }

    private void ShowCompleted()
    {
        tutorialTitle.text = "Step 11: Head Back To Your Tent";
        tutorialText.text = "It's time for some rest after a hard day's work!\n\n" +
                           "Head back to the tent and press E to call it a day!\n";
    }

    private void CheckStepCompletion()
    {
        TutorialStep currentTutorialStep = tutorialSteps[currentStep];
        bool stepComplete = false;
        
        switch (currentTutorialStep)
        {
            case TutorialStep.Welcome:
                break;
                
            case TutorialStep.GoToTent:
                if (IsPlayerNear(tentObject, 4f))
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.SetupTent:
                if (interactableManager.GetTentSetup())
                {
                    stepComplete = true;
                }
                break;

            case TutorialStep.GoToCampfire:
                if (IsPlayerNear(campfireObject, 2f) && Input.GetKeyDown(KeyCode.E))
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.CollectTinder:
                if (fireResourceManager != null && fireResourceManager.GetTinderCount() >= 3)
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.CollectKindling:
                if (fireResourceManager != null && fireResourceManager.GetKindlingCount() >= 3)
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.CollectLogs:
                if (fireResourceManager != null && fireResourceManager.GetLogsCount() >= 3)
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.GoBackToCampfire:
                if (IsPlayerNear(campfireObject, 2f))
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.BuildFire:
                if (interactableManager.GetCampfireLit())
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.CollectMushrooms:
                if (cookingManager != null && cookingManager.GetMushroomCount() >= 5)
                {
                    stepComplete = true;
                }
                break;
                
            case TutorialStep.CookMeal:
                if (interactableManager.GetFoodCooked())
                {
                    stepComplete = true;
                }
                break;
            case TutorialStep.Complete:
                ShowCompleted();
                break;
        }
        
        if (stepComplete)
        {
            NextStep();
        }
    }

    private void HighlightObject(GameObject obj)
    {
        if (obj == null) return;
        
        ObjectHighlighter highlighter = obj.GetComponent<ObjectHighlighter>();
        if (highlighter == null)
        {
            highlighter = obj.AddComponent<ObjectHighlighter>();
        }
        
        highlighter.StartGlowing();
    }

    private void ClearAllHighlights()
    {
        ObjectHighlighter[] highlighters = FindObjectsOfType<ObjectHighlighter>();
        foreach (ObjectHighlighter h in highlighters)
        {
            h.StopGlowing();
        }
    }

    private bool IsPlayerNear(GameObject target, float distance)
    {
        if (player == null || target == null) return false;
        
        float dist = Vector3.Distance(player.transform.position, target.transform.position);
        return dist <= distance;
    }

    private IEnumerator AutoAdvanceAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        NextStep();
    }

    public void NextStep()
    {
        ++currentStep;
        UpdateStepCounter();
        Debug.Log($"Check Counter: {currentStep}");
        
        if (currentStep >= tutorialSteps.Length)
        {
            CompleteTutorial();
        }
        else
        {
            ShowCurrentStep();
        }
    }

    private void UpdateStepCounter()
    {
        if (stepCounter != null)
        {
            stepCounter.text = $"Step {currentStep}/{tutorialSteps.Length}";
        }
    }

    private void CompleteTutorial()
    {
        tutorialComplete = true;
        
        tutorialTitle.text = "Tutorial Complete!";
        tutorialText.text = "Congratulations! You've learned:\n\n" +
                           "Proper tent setup\n" +
                           "Safe fire building\n" +
                           "Resource collection\n" +
                           "Outdoor cooking\n\n" +
                           "Remember to always practice Leave No Trace principles!\n\n" +
                           "Enjoy your camping adventure!";
        
        ClearAllHighlights();
        
        StartCoroutine(ShowCreditsAfterDelay(5f));
    }

    private IEnumerator ShowCreditsAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
        
        if (GameSceneManager.Instance != null)
        {
            Debug.Log("Tutorial complete - loading credits");
            GameSceneManager.Instance.LoadCredits();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        }
    }
}