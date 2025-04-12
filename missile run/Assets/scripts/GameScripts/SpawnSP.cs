using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSP : MonoBehaviour
{
    [SerializeField] GameObject indicatorPrefab;
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject[] sp;

    private List<GameObject> activeSPs = new List<GameObject>();
    private float timeGap = 10f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeGap);

            // Clean up nulls (destroyed SPs)
            activeSPs.RemoveAll(sp => sp == null);

            if (activeSPs.Count < 3)
            {
                SpawnSPWithIndicator();
            }
        }
    }

    void SpawnSPWithIndicator()
    {
        int rand = Random.Range(0, sp.Length);
        Vector3 whereToSpawn = new Vector3(Random.Range(-80f, 80f), Random.Range(-110f, 110f));
        GameObject newSpecialPower = Instantiate(sp[rand], whereToSpawn, Quaternion.identity);

        // Track the new SP
        activeSPs.Add(newSpecialPower);

        // Get the SPInfo component from the SP (holds custom sprite)
        SPInfo spInfo = newSpecialPower.GetComponent<SPInfo>();

        // Create the indicator
        GameObject newIndicator = Instantiate(indicatorPrefab, canvas.transform);
        SPIndicator indicatorScript = newIndicator.GetComponent<SPIndicator>();
        indicatorScript.target = newSpecialPower.transform;
        indicatorScript.canvasRect = canvas.GetComponent<RectTransform>();

        // Set custom sprite if available
        if (spInfo != null && spInfo.indicatorSprite != null)
        {
            indicatorScript.indicatorImage.sprite = spInfo.indicatorSprite;
        }
    }
}
