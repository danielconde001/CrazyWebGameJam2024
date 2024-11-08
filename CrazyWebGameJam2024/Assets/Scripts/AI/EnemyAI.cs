using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public enum EnemyState
{
    NULL,
    WANDER,
    ATTACK,
    CHASE,
    SEARCH,
    PATROL,
    TURRET,
    STATIONARY
}

public class EnemyAI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] protected Transform basePivot;
    [SerializeField] protected Transform headPivot;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected Animator selfAnimator;
    [SerializeField] protected Weapon selfWeapon;
    [SerializeField] protected Collider2D selfCapsuleCollider;

    [Header("Detection")]
    [SerializeField] protected LayerMask detectionLayers;
    [SerializeField] protected string playerTag;
    [SerializeField] protected float initialViewDistance;
    [SerializeField] protected float aggroViewDistance;
    [SerializeField] protected float initialFieldOfVision;
    [SerializeField] protected float aggroFieldOfVision;
    [SerializeField] protected float turnSpeed;
    [SerializeField] protected bool canDetect = true;

    [Header("State")]
    [SerializeField] protected EnemyState startingEnemyState;
    [SerializeField] protected float minMoveDistance;
    [SerializeField] protected float minPatrolDistance;
    [SerializeField] protected float initialSpeed;
    [SerializeField] protected float minWanderDistance;
    [SerializeField] protected float maxWanderDistance;
    [SerializeField] protected float minWanderDelay;
    [SerializeField] protected float maxWanderDelay;
    [SerializeField] protected float firstAttackDelay;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float chaseStopDelay;
    [SerializeField] protected float searchDuration;
    [SerializeField] protected List<Transform> patrolPoints;
    [SerializeField] protected List<AudioClip> enemyDeathSFXList;    
    
    protected Sequence searchSequence;
    protected Transform rootTransform;
    protected Transform playerTransform; 
    protected Vector3 lastPlayerPosition;
    protected Vector3 randomWanderPos;
    protected Vector3 initialPosition;
    protected int currentPatrolPoint = 0;
    protected float timer = 0.0f;
    protected bool isAIStopped = false;
    protected bool isPlayerSpotted = false;
    protected SpriteRenderer spriteRenderer;
    
    protected EnemyState currentEnemyState;
    public virtual EnemyState CurrentEnemyState
    {
        get {return currentEnemyState;}
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
                    if(searchSequence != null)
                    {
                        searchSequence.Kill();
                    }

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
                    randomWanderPos = GetRandomPosition();
                    navMeshAgent.speed = initialSpeed;
                    navMeshAgent.SetDestination(randomWanderPos);
                    timer = Random.Range(minWanderDelay, maxWanderDelay);
                    isPlayerSpotted = false;

                    break;
                }
                case EnemyState.ATTACK:
                {
                    EventsManager.Instance().PlayerSpotted();
                    navMeshAgent.ResetPath();
                    timer = firstAttackDelay;
                    isPlayerSpotted = true;
                    
                    break;
                }
                case EnemyState.CHASE:
                {
                    lastPlayerPosition = RandomizePosition(playerTransform.position, 0.5f);
                    navMeshAgent.speed = chaseSpeed;
                    navMeshAgent.SetDestination(lastPlayerPosition);
                    timer = chaseStopDelay;

                    break;
                }
                case EnemyState.SEARCH:
                {
                    if(searchSequence != null)
                    {
                        searchSequence.Kill();
                    }
                    
                    searchSequence = DOTween.Sequence();
                    Vector3 newLeftRot = new Vector3(headPivot.localRotation.eulerAngles.x - Random.Range(20.0f, 60.0f), headPivot.localRotation.eulerAngles.y, headPivot.localRotation.eulerAngles.z);
                    Vector3 newRightRot = new Vector3(headPivot.localRotation.eulerAngles.x + Random.Range(20.0f, 60.0f), headPivot.localRotation.eulerAngles.y, headPivot.localRotation.eulerAngles.z);
                    searchSequence.Append(headPivot.DOLocalRotate(newLeftRot, searchDuration / 2.0f, RotateMode.FastBeyond360));
                    searchSequence.Append(headPivot.DOLocalRotate(newRightRot, searchDuration / 2.0f, RotateMode.FastBeyond360));
                    searchSequence.Play();
                    timer = searchDuration;

                    break;
                }
                case EnemyState.PATROL:
                {
                    navMeshAgent.speed = initialSpeed;
                    currentPatrolPoint = 0;
                    isPlayerSpotted = false;

                    break;
                }
                case EnemyState.TURRET:
                {
                    isPlayerSpotted = false;
                    
                    break;
                }
                case EnemyState.STATIONARY:
                {
                    navMeshAgent.speed = initialSpeed;
                    navMeshAgent.SetDestination(initialPosition);
                    isPlayerSpotted = false;

                    break;
                }
            }
        }
    }

    public virtual void GameOver()
    {
        canDetect = false;

        if(startingEnemyState != EnemyState.PATROL
        && startingEnemyState != EnemyState.TURRET
        && startingEnemyState != EnemyState.STATIONARY)
        {
            CurrentEnemyState = EnemyState.CHASE;
        }
        else
        {
            CurrentEnemyState = startingEnemyState;
        }
    }

    public void StopAI()
    {
        if(searchSequence != null)
        {
            searchSequence.Kill();
        }

        AudioManager.Instance().PlaySFX(enemyDeathSFXList[Random.Range(0, enemyDeathSFXList.Count)], 0.25f);
        selfCapsuleCollider.enabled = false;
        isAIStopped = true;
        navMeshAgent.isStopped = true;
        CurrentEnemyState = EnemyState.NULL;
    }

    public void PlayerAlert()
    {
        if(startingEnemyState != EnemyState.TURRET)
        {
            if(CurrentEnemyState != EnemyState.ATTACK)
            {
                CurrentEnemyState = EnemyState.CHASE;
            }
        }
        else
        {
            StartCoroutine(LookAtPosition(playerTransform.position));
        }
    }

    protected virtual void Awake()
    {
        rootTransform = transform;
        initialPosition = basePivot.position;
        CurrentEnemyState = startingEnemyState;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Start()
    {
        playerTransform = PlayerManager.Instance().GetPlayer().transform;
        EventsManager.Instance().OnPlayerSpotted.AddListener(PlayerAlert);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance().HasLevelStarted() == false) return;
        
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

            selfWeapon.GetSpriteRenderer().flipY = headPivot.forward.x < 0.0f;

            UpdateStateChecker();

            if(canDetect == true)
            {
                DetectionChecker();
            }
        }
    }

    protected Vector3 GetRandomPosition()
    {
        Vector3 randomPos = new Vector3(basePivot.position.x + Random.Range(minWanderDistance, maxWanderDistance), 
                                        basePivot.position.y + Random.Range(minWanderDistance, maxWanderDistance), 
                                        basePivot.position.z);
        
        NavMeshPath path = new NavMeshPath();
        if(Physics2D.Raycast(basePivot.position, (randomPos - basePivot.position).normalized, Vector2.Distance(basePivot.position, randomPos)) == false)
        {
            if(navMeshAgent.CalculatePath(randomPos, path))
            {
                if(path.status == NavMeshPathStatus.PathInvalid || path.status == NavMeshPathStatus.PathPartial)
                {
                    return GetRandomPosition();
                }
                else if(path.status == NavMeshPathStatus.PathComplete)
                {
                    return randomPos;
                }
            }
            else
            {
                return GetRandomPosition();
            }
        }
        else
        {
            return GetRandomPosition();
        }

        return GetRandomPosition();
    }

    protected Vector3 RandomizePosition(Vector3 position, float randomness)
    {
        Vector3 randomPos = new Vector3(position.x + Random.Range(-randomness, randomness),
                                        position.y + Random.Range(-randomness, randomness),
                                        position.z);

        NavMeshPath path = new NavMeshPath();

        if(navMeshAgent.CalculatePath(randomPos, path))
        {
            if(path.status == NavMeshPathStatus.PathInvalid || path.status == NavMeshPathStatus.PathPartial)
            {
                return RandomizePosition(position, randomness);
            }
            else if(path.status == NavMeshPathStatus.PathComplete)
            {
                return randomPos;
            }
        }
        else
        {
            return RandomizePosition(position, randomness);
        }

        return RandomizePosition(position, randomness);
    }

    protected virtual void UpdateStateChecker()
    {
        switch(currentEnemyState)
        {
            case EnemyState.WANDER:
            {
                if(navMeshAgent.remainingDistance > minMoveDistance && navMeshAgent.velocity.magnitude != 0.0f)
                {
                    Quaternion rot = Quaternion.LookRotation(navMeshAgent.velocity.normalized, Vector3.back);
                    headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                }
                else if(navMeshAgent.remainingDistance <= minMoveDistance)
                {
                    if(navMeshAgent.hasPath == true)
                    {
                        navMeshAgent.ResetPath();
                    }
            
                    if(timer >= 0.0f)
                    {
                        timer -= Time.deltaTime;
                        if(timer <= 0.0f)
                        {
                            randomWanderPos = GetRandomPosition();
                            navMeshAgent.speed = initialSpeed;
                            navMeshAgent.SetDestination(randomWanderPos);
                            timer = Random.Range(minWanderDelay, maxWanderDelay);
                        }
                    }
                }

                break;
            }
            case EnemyState.ATTACK:
            {
                if(timer >= 0.0f)
                {
                    timer -= Time.deltaTime;
                    if(timer <= 0.0f)
                    {
                        Vector3 playerPosOffset = new Vector3(playerTransform.position.x, playerTransform.position.y + headPivot.localPosition.y, playerTransform.position.z);
                        Quaternion rot = Quaternion.LookRotation((playerPosOffset - headPivot.position).normalized, Vector3.back);
                        headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                        
                        RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(headPivot.position, headPivot.forward, aggroViewDistance, detectionLayers);

                        for (int i = 0; i < raycastHit2D.Length; i++)
                        {
                            if (raycastHit2D[i].collider.gameObject.CompareTag(playerTag) == true)
                            {
                                if(raycastHit2D[i].collider != null)
                                {
                                    if(raycastHit2D[i].collider.gameObject.tag == playerTag)
                                    {
                                        selfWeapon.Fire();
                                    }
                                }
                            }
                        }
                        
                    }
                }

                break;
            }
            case EnemyState.CHASE:
            {
                if(navMeshAgent.remainingDistance > minMoveDistance && navMeshAgent.velocity.magnitude != 0.0f)
                {
                    Quaternion rot = Quaternion.LookRotation(navMeshAgent.velocity.normalized, Vector3.back);
                    headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                }
                else if(navMeshAgent.remainingDistance <= minMoveDistance)
                {   
                    if(navMeshAgent.hasPath == true)
                    {
                        navMeshAgent.ResetPath();
                    }

                    if(timer >= 0.0f)
                    {
                        timer -= Time.deltaTime;
                        if(timer <= 0.0f)
                        {
                            timer = 0.0f;
                            CurrentEnemyState = EnemyState.SEARCH;
                        }
                    }
                }

                break;
            }
            case EnemyState.SEARCH:
            {
                if(timer >= 0.0f)
                {
                    timer -= Time.deltaTime;
                    if(timer <= 0.0f)
                    {
                        timer = 0.0f;

                        CurrentEnemyState = startingEnemyState;
                    }
                }

                break;
            }
            case EnemyState.PATROL:
            {
                if(navMeshAgent.hasPath == false)
                {
                    navMeshAgent.SetDestination(patrolPoints[currentPatrolPoint].position);
                }
                else
                {
                    if(navMeshAgent.remainingDistance > minPatrolDistance && navMeshAgent.velocity.magnitude != 0.0f)
                    {
                        Quaternion rot = Quaternion.LookRotation(navMeshAgent.velocity.normalized, Vector3.back);
                        headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                    }
                    else if(navMeshAgent.remainingDistance <= minPatrolDistance)
                    {
                        if(navMeshAgent.hasPath == true)
                        {
                            currentPatrolPoint++;

                            if(currentPatrolPoint >= patrolPoints.Count)
                            {
                                currentPatrolPoint = 0;
                            }

                            navMeshAgent.ResetPath();
                        }
                    }
                }

                break;
            }
            case EnemyState.TURRET:
            {
                    
                break;
            }
            case EnemyState.STATIONARY:
            {
                if(navMeshAgent.remainingDistance > minMoveDistance && navMeshAgent.velocity.magnitude != 0.0f)
                {
                    Quaternion rot = Quaternion.LookRotation(navMeshAgent.velocity.normalized, Vector3.back);
                    headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                }
                else if(navMeshAgent.remainingDistance <= minMoveDistance)
                {   
                    if(navMeshAgent.hasPath == true)
                    {
                        navMeshAgent.ResetPath();
                    }
                }

                break;
            }
        }
    }

    protected void DetectionChecker()
    {
        if(Vector3.Distance(basePivot.position, playerTransform.position) < (isPlayerSpotted == false ? initialViewDistance : aggroViewDistance))
        {
            Vector3 playerDirection = (playerTransform.position - basePivot.position).normalized;
            if(Vector3.Angle(headPivot.forward, playerDirection) < ((isPlayerSpotted == false ? initialFieldOfVision : aggroFieldOfVision) / 2.0f))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(headPivot.position, playerDirection, (isPlayerSpotted == false ? initialViewDistance : aggroViewDistance), detectionLayers);
                if(raycastHit2D.collider != null)
                {
                    if(raycastHit2D.collider.gameObject.tag == playerTag)
                    {
                        CurrentEnemyState = EnemyState.ATTACK;
                    }
                    else
                    {
                        if(startingEnemyState != EnemyState.TURRET)
                        {
                            if(CurrentEnemyState == EnemyState.ATTACK)
                            {
                                CurrentEnemyState = EnemyState.CHASE;
                            }
                        }
                        else
                        {
                            CurrentEnemyState = EnemyState.TURRET;
                        }
                    }
                }
            }
            else
            {
                if(startingEnemyState != EnemyState.TURRET)
                {
                    if(CurrentEnemyState == EnemyState.ATTACK)
                    {
                        CurrentEnemyState = EnemyState.CHASE;
                    }
                }
                else
                {
                    CurrentEnemyState = EnemyState.TURRET;
                }
            }
        }
        else
        {
            if(startingEnemyState != EnemyState.TURRET)
            {
                if(CurrentEnemyState == EnemyState.ATTACK)
                {
                    CurrentEnemyState = EnemyState.CHASE;
                }
            }
            else
            {
                CurrentEnemyState = EnemyState.TURRET;
            }
        }
    }

    protected IEnumerator LookAtPosition(Vector3 position)
    {
        Quaternion rot = Quaternion.LookRotation((position - headPivot.position).normalized, Vector3.back);
        
        while(Vector3.Angle(headPivot.forward, (position - headPivot.position).normalized) > 1.0f)
        {
            headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    protected void BasePivotFix()
    {
        basePivot.localPosition = new Vector3(basePivot.localPosition.x, basePivot.localPosition.y, -rootTransform.position.z);
    }
}
