using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    private GameObject player;
    private Vector2 moveDirection;
    private bool isStopped = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        player = GameManager.Instance().GetPlayer();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isStopped || GameManager.Instance().PlayerIsDead())
        {
            animator.SetFloat("Velocity", 0f);
            return;
        }

        moveDirection = (player.transform.position - transform.position).normalized;
        moveDirection = Vector2.ClampMagnitude(moveDirection, 1);

        animator.SetFloat("Velocity", moveDirection.magnitude);
        spriteRenderer.flipX = moveDirection.x > 0f ? false : moveDirection.x < 0f ? true : spriteRenderer.flipX;

        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;
    }

    public void StopAI()
    {
        isStopped = true;
    }
}
