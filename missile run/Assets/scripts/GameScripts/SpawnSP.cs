using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSP : MonoBehaviour
{
    [SerializeField] GameObject[] sp;
    public float CURRENT_SP;
    private float Time_Gap = 10f;

    void Update()
    {
        if (CURRENT_SP <= 3)
        {
            CURRENT_SP += 1;
            spawnSP();
        }
    }

    public void spawnSP()
    {
        int rand = Random.Range(0, sp.Length);
        StartCoroutine(spawnSpecialPower(Time_Gap, sp[rand]));
    }

    public IEnumerator spawnSpecialPower(float Time_Gap, GameObject SP)
    {
        yield return new WaitForSeconds(Time_Gap);

        Vector3 whereToSpawn = new Vector3(Random.Range(-80f, 80f), Random.Range(-110f, 110f));

        GameObject newSpecialPower = Instantiate(SP, whereToSpawn, Quaternion.identity);

    }
}
