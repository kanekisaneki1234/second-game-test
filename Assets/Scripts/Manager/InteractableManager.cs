using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance { get; private set; }
    [Header("Progress Tracking")]
    [SerializeField] private bool tentIsSetup = false;
    [SerializeField] private bool campfireIsLit = false;
    [SerializeField] private bool foodIsCooked = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    
    }

    public bool GetTentSetup()
    {
        return tentIsSetup;
    }
    public bool GetCampfireLit()
    {
        return campfireIsLit;
    }
    public bool GetFoodCooked()
    {
        return foodIsCooked;
    }


    public void SetTentSetup(bool isSetup)
    {
        tentIsSetup = isSetup;
        if (isSetup)
        {
            EnableCampfireResources();
        }
    }
    public void SetCampfireLit(bool isLit)
    {
        campfireIsLit = isLit;
        if (isLit)
        {
            EnableCookingResources();
        }
    }
    public void SetFoodCooked(bool isCooked)
    {
        foodIsCooked = isCooked;
    }

    public bool CanCallItADay()
    {
        return tentIsSetup && campfireIsLit && foodIsCooked;
    }

    public void EnableCampfireResources()
    {
        // EnableResourcesByTag("Tinder");
        // EnableResourcesByTag("Kindling");
        // EnableResourcesByTag("Log");
        EnableResourcesByTag("CampfireResource");
        Debug.Log("Campfire resources enabled!");
    }

    public void EnableCookingResources()
    {
        EnableResourcesByTag("CookingResource");
        Debug.Log("Cooking resources enabled!");
    }

    private void EnableResourcesByTag(string tag)
    {
        GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
        
        foreach (GameObject resource in resources)
        {
            EnableResourceCollider(resource);
        }
    }

    private void EnableResourceCollider(GameObject resource)
    {
        Collider2D collider = resource.GetComponent<Collider2D>();
        
        if (collider != null)
        {
            collider.isTrigger = true;
            // collider.enabled = true;
            Debug.Log($"Enabled collider for {resource.name}");
        }
        else
        {
            Debug.LogWarning($"No Collider2D found on {resource.name}");
        }
    }

    // private void DisableResourcesByTag(string tag)
    // {
    //     GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
        
    //     foreach (GameObject resource in resources)
    //     {
    //         Collider2D collider = resource.GetComponent<Collider2D>();
            
    //         if (collider != null)
    //         {
    //             collider.enabled = false;
    //             Debug.Log($"Disabled collider for {resource.name} - inventory full!");
    //         }
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
