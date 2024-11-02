using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float minTimer;
    [SerializeField] private float maxTimer;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private bool spawnOnStart = true;

    private static GameManager instance;
    public static GameManager Instance()
    {
        if (instance == null)
        {
            GameObject gameManager = new GameObject("GameManager");
            instance = gameManager.AddComponent<GameManager>();
        }
        return instance;
    }

    private void Awake()
    {
        instance = this;
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