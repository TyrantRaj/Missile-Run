using UnityEngine;

public class BigCoin : MonoBehaviour
{
    public GameObject smallCoinPrefab; 
    public int smallCoinCount = 8;      
    public float spawnRadius = 1.5f;   

    public void SpawnSmallCoinsAndDestroy()
    {
        Vector3 center = transform.position;

        for (int i = 0; i < smallCoinCount; i++)
        {
            float angle = i * Mathf.PI * 2f / smallCoinCount;  // evenly spaced angle
            float x = Mathf.Cos(angle) * spawnRadius;
            float y = Mathf.Sin(angle) * spawnRadius;
            Vector3 spawnPos = new Vector3(center.x + x, center.y + y, center.z);

            Instantiate(smallCoinPrefab, spawnPos, Quaternion.identity);
        }

        Destroy(gameObject); 
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnSmallCoinsAndDestroy();
        }
    }
}
