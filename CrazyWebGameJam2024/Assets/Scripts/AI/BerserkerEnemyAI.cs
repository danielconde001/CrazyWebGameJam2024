using UnityEngine;

public class BerserkerEnemyAI : EnemyAI
{
    public override EnemyState CurrentEnemyState 
    { 
        get => base.CurrentEnemyState;
        set
        {            
            EnemyState oldEnemyState = currentEnemyState;

            switch(oldEnemyState)
            {
                case EnemyState.WANDER:
                {
                    
                    break;
                }
                case EnemyState.ATTACK:
                {

                    break;
                }
                case EnemyState.CHASE:
                {
                    
                    break;
                }
                case EnemyState.SEARCH:
                {

                    break;
                }
                case EnemyState.PATROL:
                {

                    break;
                }
                case EnemyState.TURRET:
                {
                    
                    break;
                }
                case EnemyState.STATIONARY:
                {

                    break;
                }
            }

            currentEnemyState = value;

            switch(currentEnemyState)
            {
                case EnemyState.WANDER:
                {

                    break;
                }
                case EnemyState.ATTACK:
                {

                    break;
                }
                case EnemyState.CHASE:
                {
                    navMeshAgent.speed = chaseSpeed;

                    break;
                }
                case EnemyState.SEARCH:
                {

                    break;
                }
                case EnemyState.PATROL:
                {

                    break;
                }
                case EnemyState.TURRET:
                {
                    
                    break;
                }
                case EnemyState.STATIONARY:
                {

                    break;
                }
            }
        }
    }

    public override void GameOver()
    {
        canDetect = false;
        CurrentEnemyState = EnemyState.NULL;
    }

    protected override void Awake()
    {
        startingEnemyState = EnemyState.CHASE;

        base.Awake();
    }

    protected override void Update()
    {
        BasePivotFix();

        if(PlayerManager.Instance().PlayerIsDead() == true && canDetect == true)
        {
            GameOver();
        }

        if(isAIStopped == false)
        {
            if((navMeshAgent.velocity.normalized.y != 0 || navMeshAgent.velocity.normalized.x != 0) && navMeshAgent.hasPath == true)
            {
                selfAnimator.SetBool("isMoving", true);
                spriteRenderer.flipX = navMeshAgent.velocity.x < 0;
            }
            else
            {
                selfAnimator.SetBool("isMoving", false);
            }

            UpdateStateChecker();
        }
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == playerTag)
        {
            other.gameObject.GetComponent<Health>().DeductHealth(1);
        }
    }

    protected override void UpdateStateChecker()
    {
        switch(currentEnemyState)
        {
            case EnemyState.WANDER:
            {

                break;
            }
            case EnemyState.ATTACK:
            {

                break;
            }
            case EnemyState.CHASE:
            {
                navMeshAgent.SetDestination(playerTransform.position);

                break;
            }
            case EnemyState.SEARCH:
            {

                break;
            }
            case EnemyState.PATROL:
            {

                break;
            }
            case EnemyState.TURRET:
            {
                    
                break;
            }
            case EnemyState.STATIONARY:
            {

                break;
            }
        }
    }
}
