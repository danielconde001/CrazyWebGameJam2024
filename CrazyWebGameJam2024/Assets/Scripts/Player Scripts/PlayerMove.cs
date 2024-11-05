using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMove : PlayerControl
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private Bounds bounds;
    private Rigidbody2D selfRigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;

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
        if(!canControl)
            return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        if (moveY != 0 || moveX != 0)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        moveDirection = new Vector3(moveX, moveY).normalized;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);
        
        selfRigidbody2D.linearVelocity = moveDirection * moveSpeed * Time.fixedDeltaTime;

        #region Bounds
        if (bounds != null)
        {
            if (transform.position.x > bounds.xMax) transform.position = new Vector2(bounds.xMax, transform.position.y);
            if (transform.position.x < bounds.xMin) transform.position = new Vector2(bounds.xMin, transform.position.y);
            if (transform.position.y > bounds.yMax) transform.position = new Vector2(transform.position.x, bounds.yMax);
            if (transform.position.y < bounds.yMin) transform.position = new Vector2(transform.position.x, bounds.yMin);
        }
        #endregion
    }
}
