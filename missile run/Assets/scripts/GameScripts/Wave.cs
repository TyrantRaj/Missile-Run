using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] GameObject wave;

    [SerializeField]
    private GameObject Wavemissile;
    private float WaveTimeinterval = 10f;
    [SerializeField]
    private Transform Player_transform;


    // Update is called once per frame
    void Update()
    {
        StartCoroutine(spawnEnemyWave(WaveTimeinterval, Wavemissile));
    }

    private IEnumerator spawnEnemyWave(float WaveTimeinterval, GameObject up_missile)
    {
        yield return new WaitForSeconds(WaveTimeinterval);
        wave.transform.position = Player_transform.position + new Vector3(10,10,10);
        GameObject gameobject = Instantiate(Wavemissile, transform.position, Quaternion.identity);
        StartCoroutine(spawnEnemyWave(WaveTimeinterval, Wavemissile));

    }
}
