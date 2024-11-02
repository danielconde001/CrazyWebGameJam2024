using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float minTimer;
    [SerializeField] private float maxTimer;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private bool spawnOnStart;

    private Transform weaponAnchor;
    public Transform WeaponAnchor
    {
        get => weaponAnchor;
    }

    public GameObject Player
    {
        get => player;
    }
    
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
        
        player = GameObject.FindGameObjectWithTag("Player");
        weaponAnchor = player.transform.Find("weapon_anchor");
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

            if (PlayerIsDead())
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

    public GameObject GetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        return player;
    }

    public bool PlayerIsDead()
    {
        return player.GetComponent<PlayerKillable>().IsKilled();
    }

    public void MoveCrosshair(Vector3 newPosition)
    {
        if (!crosshair.activeInHierarchy)
            crosshair.SetActive(true);

        crosshair.transform.position = newPosition;
    }

    public void ReplaceEquippedGunWith()
    {
        //GameObject gunPrefab = GetGunPrefab(pGunType);
        //GameObject weapon = (GameObject)Instantiate(GetGunPrefab(pGunType), gunPrefab.transform.position, Quaternion.identity);
        //weapon.transform.SetParent(weaponAnchor, false);
    }
}