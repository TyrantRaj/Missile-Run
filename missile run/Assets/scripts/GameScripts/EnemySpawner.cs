using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Missile Prefabs")]
    [SerializeField] private GameObject homingMissilePrefab;
    [SerializeField] private GameObject oneHitMissilePrefab;
    [SerializeField] private GameObject waveMissilePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float spawnDistance = 30f;

    [Header("Spawn Timers (seconds)")]
    [SerializeField] private float homingSpawnRate = 4f;
    [SerializeField] private float oneHitSpawnRate = 10f;
    [SerializeField] private float waveSpawnRate = 6f;

    [Header("Difficulty Scaling")]
    [SerializeField] private float difficultyIncreaseInterval = 15f;
    [SerializeField] private float spawnRateMultiplier = 0.9f; // reduces interval
    [SerializeField] private int startingMissileCap = 15;
    [SerializeField] private int missileCapIncrease = 5;

    private float homingTimer, oneHitTimer, waveTimer, difficultyTimer;
    private int currentMissileCap;
    private List<GameObject> activeMissiles = new List<GameObject>();

    public bool canSpawn = true;

    void Start()
    {
        homingTimer = homingSpawnRate;
        oneHitTimer = oneHitSpawnRate;
        waveTimer = waveSpawnRate;
        difficultyTimer = difficultyIncreaseInterval;

        currentMissileCap = startingMissileCap;
    }

    void Update()
    {
        if (!canSpawn || playerTransform == null) return;

        homingTimer -= Time.deltaTime;
        oneHitTimer -= Time.deltaTime;
        waveTimer -= Time.deltaTime;
        difficultyTimer -= Time.deltaTime;

        if (activeMissiles.Count < currentMissileCap)
        {
            if (homingTimer <= 0f)
            {
                SpawnMissile(homingMissilePrefab, MissileType.Homing);
                homingTimer = homingSpawnRate;
            }

            if (oneHitTimer <= 0f)
            {
                SpawnMissile(oneHitMissilePrefab, MissileType.OneHit);
                oneHitTimer = oneHitSpawnRate;
            }

            if (waveTimer <= 0f)
            {
                SpawnMissile(waveMissilePrefab, MissileType.Wave);
                waveTimer = waveSpawnRate;
            }
        }

        if (difficultyTimer <= 0f)
        {
            ScaleDifficulty();
            difficultyTimer = difficultyIncreaseInterval;
        }
    }

    private void ScaleDifficulty()
    {
        homingSpawnRate *= spawnRateMultiplier;
        oneHitSpawnRate *= spawnRateMultiplier;
        waveSpawnRate *= spawnRateMultiplier;
        currentMissileCap += missileCapIncrease;
    }

    private enum MissileType { Homing, OneHit, Wave }

    private void SpawnMissile(GameObject missilePrefab, MissileType type)
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = playerTransform.position + (Vector3)(dir * spawnDistance);

        GameObject missile = Instantiate(missilePrefab, spawnPos, Quaternion.identity);
        activeMissiles.Add(missile);

        // Cleanup on destroy
        MissileBase baseScript = missile.GetComponent<MissileBase>();
        if (baseScript != null)
        {
            baseScript.OnMissileDestroyed += () =>
            {
                if (missile != null) activeMissiles.Remove(missile);
            };
        }

        switch (type)
        {
            case MissileType.Homing:
            case MissileType.OneHit:
                FindObjectOfType<IndicatorManager>()?.AddTarget(missile, false);
                break;

            case MissileType.Wave:
                StartCoroutine(SpawnWaveMissileWithDelay(missile, spawnPos));
                break;
        }
    }

    private IEnumerator SpawnWaveMissileWithDelay(GameObject missile, Vector3 spawnPos)
    {
        missile.SetActive(false); // Hide during warning
        yield return new WaitForSeconds(1f); // Warning duration

        if (missile != null)
        {
            missile.SetActive(true);
            missile.GetComponent<WaveMissile>().SetDirection((playerTransform.position - spawnPos).normalized);
            FindObjectOfType<IndicatorManager>()?.AddTarget(missile);
        }
    }
}
