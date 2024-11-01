using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMove : PlayerControl
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private Bounds bounds;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(!canControl)
            return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, moveY).normalized;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

        animator.SetFloat("Velocity", Vector3.ClampMagnitude(moveDirection, 1).magnitude);
        spriteRenderer.flipX = moveX > 0f ? false : moveX < 0f ? true : spriteRenderer.flipX;

        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

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
