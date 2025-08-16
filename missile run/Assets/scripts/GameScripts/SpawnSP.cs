using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSP : MonoBehaviour
{
    [SerializeField] private GameObject[] sp; // All special power prefabs
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

            // Remove destroyed/null SPs from tracking list
            activeSPs.RemoveAll(item => item == null);

            // If already have 3 active powers, skip this spawn cycle
            if (activeSPs.Count >= 3)
                continue;

            SpawnSPWithIndicator();
        }
    }

    void SpawnSPWithIndicator()
    {
        // Get all currently active SP types
        HashSet<string> activeTypes = new HashSet<string>();
        foreach (var spObj in activeSPs)
        {
            if (spObj != null)
                activeTypes.Add(spObj.name.Replace("(Clone)", "").Trim());
        }

        // Filter available prefabs to those not already active
        List<GameObject> availableSPs = new List<GameObject>();
        foreach (var prefab in sp)
        {
            if (!activeTypes.Contains(prefab.name))
                availableSPs.Add(prefab);
        }

        // If no unique SP is available, skip
        if (availableSPs.Count == 0)
            return;

        // Pick a random available SP
        GameObject chosenSP = availableSPs[Random.Range(0, availableSPs.Count)];

        // Random position near player
        Vector3 whereToSpawn = playerPos.position + new Vector3(
            Random.Range(-100f, 100f),
            Random.Range(-100f, 100f),
            0f
        );

        // Spawn and track
        GameObject newSP = Instantiate(chosenSP, whereToSpawn, Quaternion.identity);
        activeSPs.Add(newSP);

        // Register with indicator system
        FindObjectOfType<IndicatorManager>()?.AddTarget(newSP, true);
    }
}
