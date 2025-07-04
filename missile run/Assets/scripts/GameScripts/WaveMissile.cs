using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveMissile : MonoBehaviour
{
    [SerializeField] public float missile_speed = 10f;
    [SerializeField] private Rigidbody2D rb_wave;
    [SerializeField] private Animator anim;
   // [SerializeField] private GameObject Blast_ps;
    [SerializeField] private float Time_Gap = 10f;
    [SerializeField] private GameObject[] SP_gameObjects;

    private Vector2 moveDirection;
    private bool isMoving = false;

    private GameObject player;
    private PlayerMovement playerMovement;
    private GameObject SPspawn;
    private SpawnSP spawnsp;

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
        isMoving = true;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        SPspawn = GameObject.FindWithTag("SPspawn");
        spawnsp = SPspawn?.GetComponent<SpawnSP>();

        playerMovement.RightPS.Pause();
        playerMovement.LeftPS.Pause();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb_wave.velocity = moveDirection * missile_speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isMoving = false;
        rb_wave.velocity = Vector2.zero;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ShakeTrigger>()?.TriggerShake();
            Damage_Player();
            Explode();
        }
        else if (collision.CompareTag("Missile"))
        {
            ScoreManager.Instance?.AddScore(100);
            Explode();
        }
        else if (collision.CompareTag("SP"))
        {
            //spawnsp.CURRENT_SP -= 1;
            gameObject.SetActive(false);
            spawnSP();
            Invoke("Destroy_Go", 12f);
        }
    }

    private void Damage_Player()
    {
        if (playerMovement.isGod) { return; }

        if (playerMovement.both_Damaged)
        {
            Restart_Game();
        }
        else if (playerMovement.Damaged)
        {
            if (playerMovement.Damage_Side == 0)
            {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
                playerMovement.both_Damaged = true;
            }
            else
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
                playerMovement.both_Damaged = true;
            }
        }
        else
        {
            playerMovement.Damage_Side = Random.Range(0, 2);

            if (playerMovement.Damage_Side == 0)
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
            }
            else
            {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
            }

            playerMovement.Damaged = true;
        }
    }

    private void Restart_Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Explode()
    {
        FindObjectOfType<MissileIndicatorManager>()?.RemoveMissile(gameObject);

       // GameObject blast = Instantiate(Blast_ps, transform.position, Quaternion.identity);
        anim.Play("Explosion");

      //  Destroy(blast, 1f);
        Destroy(gameObject, 0.5f);
    }

    public IEnumerator spawnSpecialPower(float delay, GameObject spPrefab)
    {
        yield return new WaitForSeconds(delay);
        Vector3 spawnPos = new Vector3(Random.Range(-80f, 80f), Random.Range(-110f, 110f), 0);
        Instantiate(spPrefab, spawnPos, Quaternion.identity);
    }

    public void spawnSP()
    {
        int rand = Random.Range(0, SP_gameObjects.Length);
        StartCoroutine(spawnSpecialPower(Time_Gap, SP_gameObjects[rand]));
    }

    void Destroy_Go()
    {
        Destroy(gameObject);
    }
}
