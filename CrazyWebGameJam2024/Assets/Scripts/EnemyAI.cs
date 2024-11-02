using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    private Vector2 moveDirection;
    private bool isStopped = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    public bool IsStopped
    {
        get {return isStopped;}
    }

    private void Awake()
    {
        player = GameManager.Instance().GetPlayer();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        /*
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
        */

        if (isStopped || GameManager.Instance().PlayerIsDead())
        {
            if(navMeshAgent.isStopped != true)
            {
                navMeshAgent.isStopped = true;
            }

            animator.SetFloat("Velocity", 0f);
            return;
        }

        animator.SetFloat("Velocity", navMeshAgent.velocity.magnitude);
        spriteRenderer.flipX = moveDirection.x > 0f ? false : moveDirection.x < 0f ? true : spriteRenderer.flipX;

        navMeshAgent.SetDestination(player.transform.position);
    }

    public void StopAI()
    {
        isStopped = true;
    }
}
