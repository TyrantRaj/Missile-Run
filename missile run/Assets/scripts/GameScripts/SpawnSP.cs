using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnSP : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Canvas canvas;
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

        SPInfo spInfo = newSpecialPower.GetComponent<SPInfo>();

        StartCoroutine(SetupIndicator(newSpecialPower, spInfo));
    }

    IEnumerator SetupIndicator(GameObject spObject, SPInfo spInfo)
    {
        yield return null; // Wait one frame so SP position is initialized

        GameObject indicator = Instantiate(indicatorPrefab, canvas.transform);
        SPIndicator script = indicator.GetComponent<SPIndicator>();

        script.target = spObject.transform;
        script.canvasRect = canvas.GetComponent<RectTransform>();
        script.cam = Camera.main;
        script.distanceText = indicator.GetComponentInChildren<TextMeshProUGUI>();

        if (spInfo != null && spInfo.indicatorSprite != null)
        {
            script.indicatorImage.sprite = spInfo.indicatorSprite;
        }

        yield return new WaitForSeconds(0.5f);
        indicator.SetActive(true);
    }
}
