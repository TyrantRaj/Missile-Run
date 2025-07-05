using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSP : MonoBehaviour
{
    [SerializeField] private GameObject[] sp;
    private Transform playerPos;

    private List<GameObject> activeSPs = new List<GameObject>();
    private float timeGap = 10f;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeGap);

            // Clean up destroyed SPs
            activeSPs.RemoveAll(sp => sp == null);

            // Spawn new SP if less than 3 are active
            if (activeSPs.Count < 3)
            {
                SpawnSPWithIndicator();
            }
        }
    }

    void SpawnSPWithIndicator()
    {
        int rand = Random.Range(0, sp.Length);

        Vector3 whereToSpawn = new Vector3(
            playerPos.position.x + Random.Range(-100f, 100f),
            playerPos.position.y + Random.Range(-100f, 100f),
            0f
        );

        GameObject newSpecialPower = Instantiate(sp[rand], whereToSpawn, Quaternion.identity);
        activeSPs.Add(newSpecialPower);

        // Register with IndicatorManager (same as missile)
        FindObjectOfType<IndicatorManager>()?.AddTarget(newSpecialPower,true);
        
    }
}
