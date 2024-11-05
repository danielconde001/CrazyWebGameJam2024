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
    SEARCH
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
    [SerializeField] protected float minMoveDistance;
    [SerializeField] protected float wanderSpeed;
    [SerializeField] protected float minWanderDistance;
    [SerializeField] protected float maxWanderDistance;
    [SerializeField] protected float minWanderDelay;
    [SerializeField] protected float maxWanderDelay;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float chaseStopDelay;
    [SerializeField] protected float searchDuration;

    protected Sequence searchSequence;
    protected Transform rootTransform;
    protected Transform playerTransform; 
    protected Vector3 lastPlayerPosition;
    protected Vector3 randomWanderPos;
    protected float timer = 0.0f;
    protected bool isAIStopped = false;
    protected bool isPlayerSpotted = false;

    protected EnemyState currentEnemyState;
    public EnemyState CurrentEnemyState
    {
        get {return currentEnemyState;}
        set
        {
            if(value == currentEnemyState)
            {
                //return;
            }
            
            EnemyState oldEnemyState = currentEnemyState;
            currentEnemyState = value;

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
            }

            switch(currentEnemyState)
            {
                case EnemyState.WANDER:
                {
                    randomWanderPos = GetRandomPosition();
                    navMeshAgent.speed = wanderSpeed;
                    navMeshAgent.SetDestination(randomWanderPos);
                    timer = Random.Range(minWanderDelay, maxWanderDelay);
                    isPlayerSpotted = false;
                    break;
                }
                case EnemyState.ATTACK:
                {
                    EventsManager.Instance().PlayerSpotted();
                    navMeshAgent.ResetPath();
                    isPlayerSpotted = true;
                    break;
                }
                case EnemyState.CHASE:
                {
                    lastPlayerPosition = playerTransform.position;
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
            }
        }
    }

    public void GameOver()
    {
        canDetect = false;

        CurrentEnemyState = EnemyState.CHASE;
    }

    public void StopAI()
    {
        if(searchSequence != null)
        {
            searchSequence.Kill();
        }

        selfCapsuleCollider.enabled = false;
        isAIStopped = true;
        navMeshAgent.isStopped = true;
    }

    public void PlayerAlert()
    {
        if(CurrentEnemyState != EnemyState.ATTACK)
        {
            CurrentEnemyState = EnemyState.CHASE;
        }
    }

    protected virtual void Awake()
    {
        rootTransform = transform;
        CurrentEnemyState = EnemyState.WANDER;
    }

    protected void Start()
    {
        playerTransform = PlayerManager.Instance().GetPlayer().transform;
        EventsManager.Instance().OnPlayerSpotted.AddListener(PlayerAlert);
    }

    protected void Update()
    {
        BasePivotFix();

        if(PlayerManager.Instance().PlayerIsDead() == true && canDetect == true)
        {
            GameOver();
        }

        if(isAIStopped == false)
        {
            //THIS IS USING PLAYER ANIMATOR MAYBE CHANGE THESE
            if(navMeshAgent.velocity.normalized.y != 0 || navMeshAgent.velocity.normalized.x != 0)
            {
                selfAnimator.SetBool("isMoving", true);
                selfAnimator.SetFloat("MoveX", navMeshAgent.velocity.normalized.x);
                selfAnimator.SetFloat("MoveY", navMeshAgent.velocity.normalized.y);
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

    protected void UpdateStateChecker()
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
            
                    if(timer > 0.0f)
                    {
                        timer -= Time.deltaTime;
                        if(timer <= 0.0f)
                        {
                            randomWanderPos = GetRandomPosition();
                            navMeshAgent.speed = wanderSpeed;
                            navMeshAgent.SetDestination(randomWanderPos);
                            timer = Random.Range(minWanderDelay, maxWanderDelay);
                        }
                    }
                }
                break;
            }
            case EnemyState.ATTACK:
            {
                Vector3 playerPosOffset = new Vector3(playerTransform.position.x, playerTransform.position.y + headPivot.localPosition.y, playerTransform.position.z);
                Quaternion rot = Quaternion.LookRotation((playerPosOffset - headPivot.position).normalized, Vector3.back);
                headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                
                RaycastHit2D raycastHit2D = Physics2D.Raycast(headPivot.position, headPivot.forward, aggroViewDistance, detectionLayers);
                if(raycastHit2D.collider != null)
                {
                    if(raycastHit2D.collider.gameObject.tag == playerTag)
                    {
                        selfWeapon.Fire();
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

                    if(timer > 0.0f)
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
                if(timer > 0.0f)
                {
                    timer -= Time.deltaTime;
                    if(timer <= 0.0f)
                    {
                        timer = 0.0f;
                        CurrentEnemyState = EnemyState.WANDER;
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
                        if(CurrentEnemyState == EnemyState.ATTACK)
                        {
                            CurrentEnemyState = EnemyState.CHASE;
                        }
                    }
                }
            }
            else
            {
                if(CurrentEnemyState == EnemyState.ATTACK)
                {
                    CurrentEnemyState = EnemyState.CHASE;
                }
            }
        }
        else
        {
            if(CurrentEnemyState == EnemyState.ATTACK)
            {
                CurrentEnemyState = EnemyState.CHASE;
            }
        }
    }

    protected void BasePivotFix()
    {
        basePivot.localPosition = new Vector3(basePivot.localPosition.x, basePivot.localPosition.y, -rootTransform.position.z);
    }
}
