using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMove : PlayerControl
{
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody2D selfRigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;
    private bool isMoving = false;

    public bool IsMoving
    {
        get => isMoving;
    }

    public void StopMove()
    {
        animator.SetBool("isMoving", false);
        selfRigidbody2D.linearVelocity = Vector2.zero;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selfRigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (!canControl)
        {
            isMoving = false;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        if (moveY != 0 || moveX != 0)
        { 
            isMoving = true;
            animator.SetBool("isMoving", true);
            spriteRenderer.flipX = moveX < 0;
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }

        moveDirection = new Vector3(moveX, moveY).normalized;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);
        
        selfRigidbody2D.linearVelocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
    }
}
