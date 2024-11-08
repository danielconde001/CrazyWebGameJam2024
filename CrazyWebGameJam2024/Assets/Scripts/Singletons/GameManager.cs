using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource), typeof(TimeManipulation))]
public class GameManager : MonoBehaviour
{
    public enum LevelType
    {
        NORMAL,
        SURVIVAL,
        NONE
    }

    [SerializeField] private float minTimer;
    [SerializeField] private float maxTimer;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private AudioSource audioSource;
    private TimeManipulation timeManipulator;
    
    [Header("Level Settings")]
    [SerializeField] private LevelType levelType = LevelType.NORMAL;
    [SerializeField] private uint killCountGoal = 1;
    [SerializeField] private float survivalTimeGoal = 30f;

    [Header("Intro Settings")]
    [SerializeField] private bool hasIntro = false;
    [SerializeField] private DialogueTrigger introDialogue;
    
    private bool levelStarted = false;
    private uint currentKillCount = 0;
    private bool gameOver = false;
    private bool levelFinished = false;

    public TimeManipulation TimeManipulator
    {
        get
        { 
            if (timeManipulator == null)
                timeManipulator = GetComponent<TimeManipulation>();
            
            return timeManipulator; 
        }
    }

    private static GameManager instance;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            GameObject gameManager = Instantiate(Resources.Load("Prefabs/Singletons/GameManager", typeof(GameObject)) as GameObject);
            instance = gameManager.GetComponent<GameManager>();
        }
        return instance;
    }

    private void Awake()
    {
        instance = this;
        
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (timeManipulator == null)
            timeManipulator = GetComponent<TimeManipulation>();
    }

    private void Start()
    {
        if (hasIntro)
        {
            introDialogue.TriggerDialogue();
        }
        else
        {
            StartLevel();
        }
    }

    private void Update()
    {
        if (levelStarted == false)
        {
            if (hasIntro && HUDManager.Instance().DialogueSystem.isDialogueActive == false)
            {
                StartLevel();
            }
        }
    }

    private void StartLevel()
    {
        levelStarted = true;

        if (levelType == LevelType.SURVIVAL)
        {
            HUDManager.Instance().StartCountdownTimer(survivalTimeGoal);
            StartSpawning();
        }
    }

    private void StartSpawning()
    {
        StartCoroutine(StartSpawningCoroutine());
    }
    
    private IEnumerator StartSpawningCoroutine()
    {
        for (;;)
        {
            SpawnEnemy();
            float rnd = Random.Range(minTimer, maxTimer);
            yield return new WaitForSeconds(rnd);

            if (PlayerManager.Instance().PlayerIsDead() || levelFinished || gameOver)
            {
                yield break;
            }
        }
    }

    private void SpawnEnemy()
    {
        int rnd = Random.Range(0, spawnPoints.Count);
        Instantiate(enemyPrefab, spawnPoints[rnd]);
    }
    
    public void MoveCrosshair(Vector3 newPosition)
    {
        if (!crosshair.activeInHierarchy)
            crosshair.SetActive(true);

        crosshair.transform.position = newPosition;
    }

    public void GameOver()
    {
        gameOver = true;
        HUDManager.Instance().GameOverDisplayerDisplayer.Display();
    }

    private void FinishLevel()
    {
        levelFinished = true;
        HUDManager.Instance().LevelCompleteDisplayer.Display();
    }

    public bool HasLevelStarted()
    {
        return levelStarted;
    }
    
    public bool IsGameOver()
    {
        return gameOver;
    }
    
    public bool IsLevelFinished()
    {
        return levelFinished;
    }
    
    public void CheckLevelGoalStatus(float currentSurvivalTimer = 0f)
    {
        if (levelType == LevelType.NORMAL)
        {
            if (currentKillCount < killCountGoal)
            {
                return;
            }
            else
            {
                FinishLevel();
            }
        }
        else if (levelType == LevelType.SURVIVAL)
        {
            if (currentSurvivalTimer > 0f)
            {
                return;
            }
            else
            {
                FinishLevel();
            }
        }
    }

    public void AddKillCount(uint addValue = 1)
    {
        if (levelType != LevelType.NORMAL) return;
        
        currentKillCount += addValue;
        CheckLevelGoalStatus();
    }
}