using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject homingMissilePrefab;
    [SerializeField] private GameObject waveMissilePrefab;
    [SerializeField] private GameObject warningIndicatorPrefab;
    [SerializeField] private Transform playerTransform;

    private float homingInterval = 3.5f;
    private float waveInterval = 2f;
    private float missileSpawnDistance = 30f;
    private float warningDuration = 1f;

    void Start()
    {
        // Start both spawning routines
        StartCoroutine(SpawnHomingMissiles());
        StartCoroutine(SpawnWaveMissiles());
    }

    // === HOMING MISSILES ===
    private IEnumerator SpawnHomingMissiles()
    {
        yield return new WaitForSeconds(homingInterval);

        float angle = Random.Range(0f, 360f);
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * missileSpawnDistance;
        Vector3 spawnPosition = playerTransform.position + offset;

        GameObject missile = Instantiate(homingMissilePrefab, spawnPosition, Quaternion.identity);
        FindObjectOfType<IndicatorManager>()?.AddTarget(missile, false);

        StartCoroutine(SpawnHomingMissiles());
    }

    // === STRAIGHT WAVE MISSILES WITH WARNING ===
    private IEnumerator SpawnWaveMissiles()
    {
        yield return new WaitForSeconds(waveInterval);

        Vector2 direction = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = playerTransform.position + (Vector3)(direction * missileSpawnDistance);

        // No more world-space indicators. We use MissileIndicatorManager now.

        // Wait before spawning actual missile (acts like warning delay)
        yield return new WaitForSeconds(warningDuration);

        // Spawn wave missile
        GameObject waveMissile = Instantiate(waveMissilePrefab, spawnPosition, Quaternion.identity);
        waveMissile.GetComponent<WaveMissile>().SetDirection((playerTransform.position - spawnPosition).normalized);

        // Register with indicator system
        FindObjectOfType<IndicatorManager>()?.AddTarget(waveMissile);

        StartCoroutine(SpawnWaveMissiles());
    }

}
