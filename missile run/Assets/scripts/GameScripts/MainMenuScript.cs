using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [Header("Cloud Settings")]
    public GameObject[] cloudPrefabs;      // Assign different cloud prefabs here
    public float spawnInterval = 2f;       // Time between spawns
    public float cloudSpeed = 2f;          // Cloud movement speed
    public float spawnYMin = -2f;          // Random Y position range
    public float spawnYMax = 2f;
    public float spawnX = -10f;            // Start (left) position
    public float endX = 10f;               // End (right) position to destroy cloud

    private List<GameObject> spawnedClouds = new List<GameObject>();

    void Start()
    {
        // Pre-spawn initial clouds across the screen
        int initialCloudCount = 5; // You can adjust this number

        for (int i = 0; i < initialCloudCount; i++)
        {
            float initialX = Mathf.Lerp(spawnX, endX, (float)i / (initialCloudCount - 1));
            float randomY = Random.Range(spawnYMin, spawnYMax);
            Vector3 spawnPos = new Vector3(initialX, randomY, 0f);
            int randomIndex = Random.Range(0, cloudPrefabs.Length);

            GameObject cloud = Instantiate(cloudPrefabs[randomIndex], spawnPos, Quaternion.identity);
            //cloud.transform.localScale = new Vector2(Random.Range(0.5f,1f), Random.Range(0.5f, 1f));
            spawnedClouds.Add(cloud);
        }

        // Start spawning new clouds
        StartCoroutine(SpawnCloudsRoutine());
    }


    void Update()
    {
        // Move all clouds
        for (int i = spawnedClouds.Count - 1; i >= 0; i--)
        {
            GameObject cloud = spawnedClouds[i];
            if (cloud != null)
            {
                cloud.transform.Translate(Vector3.right * cloudSpeed * Time.deltaTime);

                if (cloud.transform.position.x > endX)
                {
                    Destroy(cloud);
                    spawnedClouds.RemoveAt(i);
                }
            }
        }
    }

    IEnumerator SpawnCloudsRoutine()
    {
        while (true)
        {
            SpawnCloud();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCloud()
    {
        float randomY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);
        int randomIndex = Random.Range(0, cloudPrefabs.Length);

        GameObject cloud = Instantiate(cloudPrefabs[randomIndex], spawnPos, Quaternion.identity);
        cloud.transform.localScale = new Vector2(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
        spawnedClouds.Add(cloud);
    }


}
