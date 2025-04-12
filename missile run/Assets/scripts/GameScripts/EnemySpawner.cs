using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject swanerPrefabs;
    private float Timeinterval = 3.5f;
    [SerializeField]
    private GameObject Wavemissile;
    private float WaveTimeinterval = 10f;
    [SerializeField]
    private Transform Player_transform;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(Timeinterval, swanerPrefabs));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        float spawnDistance = 40f; 
        float angle = Random.Range(0f, 360f); 
        Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * spawnDistance;

        Vector3 whereToSpawn = Player_transform.position + offset;

        GameObject newEnemy = Instantiate(enemy, whereToSpawn, Quaternion.identity);
        FindObjectOfType<MissileIndicatorManager>().AddMissile(newEnemy);
        StartCoroutine(spawnEnemy(interval, enemy));
    }

}
