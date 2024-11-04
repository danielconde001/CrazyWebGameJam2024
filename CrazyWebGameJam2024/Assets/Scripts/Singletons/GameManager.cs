using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(TimeManipulation))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private float minTimer;
    [SerializeField] private float maxTimer;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private AudioSource audioSource;
    private TimeManipulation timeManipulator;
    
    public AudioSource GetAudioSource()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        return audioSource;
    }

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
        if (spawnOnStart)
            StartCoroutine(StartSpawningCoroutine());
    }

    public void StartSpawning()
    { 
        if (!spawnOnStart)
            StartCoroutine(StartSpawningCoroutine());
    }
    
    private IEnumerator StartSpawningCoroutine()
    {
        for (;;)
        {
            SpawnEnemy();
            float rnd = Random.Range(minTimer, maxTimer);
            yield return new WaitForSeconds(rnd);

            if (PlayerManager.Instance().PlayerIsDead())
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
}