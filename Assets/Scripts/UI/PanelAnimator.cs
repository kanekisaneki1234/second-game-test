using UnityEngine;

public class PanelAnimator : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Vector3 targetScale = Vector3.one;
    private float animSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animSpeed);
        
        if (canvasGroup != null)
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.unscaledDeltaTime * animSpeed);
    }
}
