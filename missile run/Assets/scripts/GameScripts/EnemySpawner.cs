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
        StartCoroutine(spawnEnemy(Timeinterval,swanerPrefabs));
       // StartCoroutine(spawnEnemyWave(WaveTimeinterval, Wavemissile));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        Vector3 whereToSpawn = new Vector3(Player_transform.position.x + Random.Range(-30f, 30), Player_transform.position.y + Random.Range(-30f, 30f),0);

        while (Vector3.Distance(whereToSpawn, Player_transform.position) < 5) {
            whereToSpawn = new Vector3(Player_transform.position.x + Random.Range(-30f, 30), Player_transform.position.y + Random.Range(-30f, 30f),0);
        }

        GameObject newEnemy = Instantiate(enemy, whereToSpawn, Quaternion.identity);
        StartCoroutine(spawnEnemy(interval, enemy));   
    }
}
