using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float pickupDistance = 2f;
    private Transform player;
    private bool isAttracting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Start attracting if within pickupDistance
        if (distance < pickupDistance)
            isAttracting = true;

        if (isAttracting)
        {
            // Move towards player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Optional: destroy & add coin when very close
            if (distance < 0.2f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        CurrencyManager.instance.AddCoin(1);

        // Play sound or animation here if needed
        SoundManager.PlaySound(SoundManager.Sound.CoinPickUp);
        Destroy(gameObject);
    }
}
