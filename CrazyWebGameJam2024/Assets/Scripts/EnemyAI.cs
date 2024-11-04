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

    [SerializeField] private Transform basePivot;
    [SerializeField] private Transform headPivot;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator selfAnimator;
    [SerializeField] private SpriteRenderer selfSpriteRenderer;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private Weapon selfWeapon;

    [Header("Detection")]
    [SerializeField] private LayerMask detectionLayers;
    [SerializeField] private string playerTag;
    [SerializeField] private float initialViewDistance;
    [SerializeField] private float aggroViewDistance;
    [SerializeField] private float fieldOfVision;
    [SerializeField] private float turnSpeed;
    [SerializeField] private bool canDetect = true;

    [Header("State")]
    [SerializeField] private float minMoveDistance;
    [SerializeField] private float wanderSpeed;
    [SerializeField] private float minWanderDistance;
    [SerializeField] private float maxWanderDistance;
    [SerializeField] private float minWanderDelay;
    [SerializeField] private float maxWanderDelay;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float chaseStopDelay;
    [SerializeField] private float searchDuration;

    private Sequence searchSequence;
    private Transform rootTransform;
    private Transform playerTransform; 
    private Vector3 lastPlayerPosition;
    private Vector3 randomWanderPos;
    private float timer = 0.0f;
    private bool isAIStopped = false;
    private bool isPlayerSpotted = false;

    private EnemyState currentEnemyState;
    public EnemyState CurrentEnemyState
    {
        get {return currentEnemyState;}
        set
        {
            if(value == currentEnemyState)
            {
                return;
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
                    searchSequence.Append(headPivot.DOLocalRotate(newLeftRot, searchDuration / 2.0f, RotateMode.Fast));
                    searchSequence.Append(headPivot.DOLocalRotate(newRightRot, searchDuration / 2.0f, RotateMode.Fast));
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
        isAIStopped = true;
        navMeshAgent.isStopped = true;
    }

    private void Awake()
    {
        rootTransform = transform;
        CurrentEnemyState = EnemyState.WANDER;
        playerTransform = PlayerManager.Instance().GetPlayer().transform;
    }

    private void Update()
    {
        BasePivotFix();

        if(PlayerManager.Instance().PlayerIsDead() == true && canDetect == true)
        {
            GameOver();
        }

        if(isAIStopped == false)
        {
            selfAnimator.SetFloat("Velocity", navMeshAgent.velocity.magnitude);
            selfSpriteRenderer.flipX = navMeshAgent.velocity.x > 0f ? false : navMeshAgent.velocity.x < 0f ? true : selfSpriteRenderer.flipX;

            UpdateStateChecker();

            if(canDetect == true)
            {
                DetectionChecker();
            }
        }
    }

    private Vector3 GetRandomPosition()
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

    private void UpdateStateChecker()
    {
        switch(currentEnemyState)
        {
            case EnemyState.WANDER:
            {
                if(navMeshAgent.remainingDistance > minMoveDistance)
                {
                    Vector3 randomWanderPosOffset = new Vector3(randomWanderPos.x, randomWanderPos.y + headPivot.localPosition.y, randomWanderPos.z);
                    Quaternion rot = Quaternion.LookRotation((randomWanderPosOffset - headPivot.position).normalized, Vector3.back);
                    headPivot.rotation = Quaternion.RotateTowards(headPivot.rotation, rot, turnSpeed * Time.deltaTime);
                }
                else if(navMeshAgent.remainingDistance <= minMoveDistance)
                {
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
                Quaternion rot = Quaternion.LookRotation((playerTransform.position - headPivot.position).normalized, Vector3.back);
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
                if(navMeshAgent.remainingDistance <= minMoveDistance)
                {
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

    private void DetectionChecker()
    {
        if(Vector3.Distance(basePivot.position, playerTransform.position) < (isPlayerSpotted == false ? initialViewDistance : aggroViewDistance))
        {
            Vector3 playerDirection = (playerTransform.position - basePivot.position).normalized;
            if(Vector3.Angle(headPivot.forward, playerDirection) < (fieldOfVision / 2.0f))
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

    private void BasePivotFix()
    {
        basePivot.localPosition = new Vector3(basePivot.localPosition.x, basePivot.localPosition.y, -rootTransform.position.z);
    }
}
