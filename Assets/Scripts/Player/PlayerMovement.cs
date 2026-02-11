using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("Animator component not found on Player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        moveInput = moveInput.normalized;

        UpdateAnimations();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;
        
        float speed = moveInput.magnitude;
        
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", speed);
    }
}
