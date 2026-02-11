using System.Collections;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [Header("Glow Settings")]
    [SerializeField] private Color glowColor = Color.yellow;
    [SerializeField] private float glowIntensity = 1.5f;
    [SerializeField] private float pulseSpeed = 5f;
    
    [Header("Arrow Pointer (Optional)")]
    [SerializeField] private GameObject arrowPrefab;
    private GameObject arrowInstance;
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Material originalMaterial;
    private Material glowMaterial;
    private bool isGlowing = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            originalMaterial = spriteRenderer.material;
        }
    }

    public void StartGlowing()
    {
        if (isGlowing) return;
        
        isGlowing = true;
        StartCoroutine(GlowPulse());
        
        if (arrowPrefab != null)
        {
            arrowInstance = Instantiate(arrowPrefab, transform.position + Vector3.up * 2f, arrowPrefab.transform.rotation);
            arrowInstance.transform.SetParent(transform);
        }
    }

    public void StopGlowing()
    {
        if (!isGlowing) return;
        
        isGlowing = false;
        StopAllCoroutines();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
            spriteRenderer.material = originalMaterial;
        }
        
        if (arrowInstance != null)
        {
            Destroy(arrowInstance);
        }
    }

    private IEnumerator GlowPulse()
    {
        while (isGlowing)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            
            if (spriteRenderer != null)
            {
                Color targetColor = Color.Lerp(originalColor, glowColor * glowIntensity, pulse);
                spriteRenderer.color = targetColor;
            }
            
            yield return null;
        }
    }

    public void StartGlowingWithOutline()
    {
        if (isGlowing) return;
        isGlowing = true;
        
        StartCoroutine(GlowPulse());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
