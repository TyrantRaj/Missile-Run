using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneHitMissile : MonoBehaviour
{
    GameOver gameoverscript;
    [SerializeField] GameObject coinPrefab;
    private PlayerMovement playerMovement;
    private Transform target;
    private GameObject player;
    private Rigidbody2D missile_rb;
    public float missile_speed;
    public float rotate_speed;
    public float missile_minSpeed;
    public float missile_MaxSpeed;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        gameoverscript = GameObject.FindGameObjectWithTag("GameOver").GetComponent<GameOver>();
        playerMovement = player.GetComponent<PlayerMovement>();
        missile_rb = GetComponent<Rigidbody2D>();

        // --- Core dynamic speed logic ---
        float playerSpeed = playerMovement.orginalSpeed;

        // Adjust missile speed based on player speed
        // You can tweak multipliers to balance difficulty
        float speedMultiplier = 1.2f; // missile is 20% faster than player
        missile_speed = Mathf.Clamp(playerSpeed * speedMultiplier, missile_minSpeed, missile_MaxSpeed);

        // Adjust rotation speed based on player speed (higher speed = lower rotation, harder to track)
        float rotationBase = 150f;
        float rotationFactor = 5f;
        rotate_speed = Mathf.Clamp(rotationBase - playerSpeed * rotationFactor, 100f, 500f); // Keep rotation within bounds

        // Optional: Pause thrust particles
        playerMovement.RightPS?.Pause();
        playerMovement.LeftPS?.Pause();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        missile_rb.linearVelocity = transform.up * missile_speed;
        Vector2 direction = (target.position - transform.position).normalized;
        float rotate_amount = Vector3.Cross(transform.up, direction).z;
        missile_rb.angularVelocity = rotate_amount * rotate_speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        missile_speed = 0;
        rotate_speed = 0;

        if (collision.tag == "Player")
        {
            Damage_Player();
            Explode();

        }
        else if (collision.tag == "Missile")
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Vector3 explosionPos = collision.transform.position;
            ScoreManager.Instance?.AddScore(100, transform.position);

            Explode();
        }
        else if (collision.tag == "Border")
        {


            Explode();

        }
        else if (collision.tag == "SP")
        {
            //spawnsp.CURRENT_SP -= 1;
            gameObject.SetActive(false);
          
            Invoke("Destroy_Go", 12);
        }

    }


    private void Damage_Player()
    {
        if (playerMovement.isGod) { return; }

        if (playerMovement.both_Damaged)
        {
            Restart_Game();
            Debug.Log("Game Over");
        }
        else
        {
            playerMovement.play_right_PS = true;
            playerMovement.RightTR.enabled = false;
            playerMovement.both_Damaged = true;

            playerMovement.play_left_PS = true;
            playerMovement.LeftTR.enabled = false;
            playerMovement.both_Damaged = true;

            playerMovement.both_Damaged = true;
        }

    }

    void Restart_Game()
    {
        gameoverscript.GameOverFunction();
    }

    public IEnumerator spawnSpecialPower(float Time_Gap, GameObject SP)
    {
        yield return new WaitForSeconds(Time_Gap);

        Vector3 whereToSpawn = new Vector3(Random.Range(-80f, 80f), Random.Range(-110f, 110f));

        GameObject newSpecialPower = Instantiate(SP, whereToSpawn, Quaternion.identity);

    }

    void Explode()
    {
        
        FindObjectOfType<IndicatorManager>().RemoveTarget(gameObject);
        anim.Play("Explosion");
        SoundManager.PlaySound(SoundManager.Sound.Explosion);
        Destroy(gameObject, 0.5f); // Destroy missile itself shortly after
    }



    void Destroy_Go()
    {
        Destroy(gameObject);
    }
}

