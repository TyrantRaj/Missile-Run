using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud
{
    public GameObject cloudObj;
    public float speedMultiplier;
}


public class CloudManager : MonoBehaviour
{
    public bool isDay = false;
    [SerializeField] int maxCloudCount = 20;
    [SerializeField] GameObject[] DaycloudPrefabs;
    [SerializeField] GameObject[] NightcloudPrefabs;
    [SerializeField] Transform player;
    [SerializeField] float spawnInterval = 1.5f;
    [SerializeField] float baseCloudSpeed = 1f;
    [SerializeField] float spawnOffset = 2f;
    [SerializeField] float minParallax = 0.3f;
    [SerializeField] float maxParallax = 1.0f;

    Camera mainCam;
    Vector3 lastPlayerPos;

    List<Cloud> clouds = new List<Cloud>();

    void Start()
    {
        mainCam = Camera.main;
        lastPlayerPos = player.position;
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            SpawnCloudInMovementDirection();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCloudInMovementDirection()
    {
        if (clouds.Count >= maxCloudCount)
            return;

        Vector3 movementDir = (player.position - lastPlayerPos).normalized;
        lastPlayerPos = player.position;

        Vector3 camPos = mainCam.transform.position;
        float camHeight = 2f * mainCam.orthographicSize;
        float camWidth = camHeight * mainCam.aspect;

        Vector3 spawnPos = Vector3.zero;

        if (Mathf.Abs(movementDir.x) > Mathf.Abs(movementDir.y))
        {
            float x = movementDir.x > 0 ? camPos.x + camWidth / 2 + spawnOffset : camPos.x - camWidth / 2 - spawnOffset;
            float y = Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2);
            spawnPos = new Vector3(x, y, 0);
        }
        else
        {
            float y = movementDir.y > 0 ? camPos.y + camHeight / 2 + spawnOffset : camPos.y - camHeight / 2 - spawnOffset;
            float x = Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2);
            spawnPos = new Vector3(x, y, 0);
        }

        GameObject cloudObj;

        if (isDay)
        {
            int index = Random.Range(0, DaycloudPrefabs.Length);
            cloudObj = Instantiate(DaycloudPrefabs[index], spawnPos, Quaternion.identity);
        }
        else {
            int index = Random.Range(0, NightcloudPrefabs.Length);
            cloudObj = Instantiate(NightcloudPrefabs[index], spawnPos, Quaternion.identity);
        }
        

        float depth = Random.Range(minParallax, maxParallax);
        cloudObj.transform.localScale *= depth;

        Cloud cloud = new Cloud { cloudObj = cloudObj, speedMultiplier = depth };
        clouds.Add(cloud);
    }




    void Update()
    {
        float camLeft = mainCam.transform.position.x - mainCam.orthographicSize * mainCam.aspect - spawnOffset;
        float camRight = mainCam.transform.position.x + mainCam.orthographicSize * mainCam.aspect + spawnOffset;
        float camBottom = mainCam.transform.position.y - mainCam.orthographicSize - spawnOffset;
        float camTop = mainCam.transform.position.y + mainCam.orthographicSize + spawnOffset;

        for (int i = clouds.Count - 1; i >= 0; i--)
        {
            Cloud cloud = clouds[i];
            if (cloud.cloudObj == null)
            {
                clouds.RemoveAt(i);
                continue;
            }

            cloud.cloudObj.transform.Translate(Vector3.right * baseCloudSpeed * cloud.speedMultiplier * Time.deltaTime);

            Vector3 pos = cloud.cloudObj.transform.position;
            if (pos.x < camLeft - 10f || pos.x > camRight + 10f || pos.y < camBottom - 10f || pos.y > camTop + 10f)
            {
                Destroy(cloud.cloudObj);
                clouds.RemoveAt(i);
            }
        }
    }
}
