using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class targeting_missile : MonoBehaviour
{
    [SerializeField] GameObject Blast_ps;
    [SerializeField] float Time_Gap = 10f;
    public GameObject[] SP_gameObjects;


    private SpawnSP spawnsp;
    private GameObject SPspawn;

    private PlayerMovement playerMovement;
    private Transform target;
    private GameObject player;
    private Rigidbody2D missile_rb;
    public float missile_speed;
    public float rotate_speed;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        SPspawn = GameObject.FindWithTag("SPspawn");
        player = GameObject.FindWithTag("Player");
        target = player.transform;

        spawnsp = SPspawn.GetComponent<SpawnSP>();
        playerMovement = player.GetComponent<PlayerMovement>();

        missile_rb = GetComponent<Rigidbody2D>();
        missile_speed = Random.Range(6,8);
        playerMovement.RightPS.Pause();
        playerMovement.LeftPS.Pause();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        missile_rb.velocity = transform.up * missile_speed;
        Vector2 direction = (target.position - transform.position).normalized;
        float rotate_amount = Vector3.Cross(transform.up, direction).z;
        missile_rb.angularVelocity = rotate_amount * rotate_speed;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        missile_speed = 0;
        rotate_speed = 0;

        if(collision.tag == "Player") {
            Damage_Player();

            //anim.Play("Explosion");
            GameObject gameObject = Instantiate(Blast_ps, transform.position, Quaternion.identity);
            //Blast_ps.Play();
            //Destroy(gameObject);
            //Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else if(collision.tag == "Missile")
        {
            GameObject gameObject = Instantiate(Blast_ps, transform.position, Quaternion.identity);

            //Blast_ps.Play();
            //Destroy(gameObject);
            //anim.Play("Explosion");
            //Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else if(collision.tag == "Border")
        {
            GameObject gameObject = Instantiate(Blast_ps, transform.position, Quaternion.identity);

            //Blast_ps.Play();
            //Destroy(gameObject);
            //anim.Play("Explosion");
            //Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else if (collision.tag == "SP")
        {
            spawnsp.CURRENT_SP -= 1;
            //gameObject.SetActive(false);
            //spawnSP();
            //Invoke("Destroy_Go", 12);
        }

    }


    private void Damage_Player()
    {
        //0 means left side damage
        //1 means right side damage
        if (playerMovement.both_Damaged)
        {
            Restart_Game();
            Debug.Log("Game Over");
        }
        else if (playerMovement.Damaged)
        {
            if (playerMovement.Damage_Side == 0)
            {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
                playerMovement.both_Damaged = true;

            }else if(playerMovement.Damage_Side == 1)
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
                playerMovement.both_Damaged = true;
            }
        }
        else
        {
            playerMovement.Damage_Side = UnityEngine.Random.Range(0, 1);
            if(playerMovement.Damage_Side == 0)
            {
                playerMovement.play_left_PS = true;
                playerMovement.LeftTR.enabled = false;
            }else {
                playerMovement.play_right_PS = true;
                playerMovement.RightTR.enabled = false;
            }
            playerMovement.Damaged = true;
        }
    }

    void Restart_Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator spawnSpecialPower(float Time_Gap, GameObject SP)
    {
        yield return new WaitForSeconds(Time_Gap);

        Vector3 whereToSpawn = new Vector3(Random.Range(-80f, 80f), Random.Range(-110f, 110f));

        GameObject newSpecialPower = Instantiate(SP, whereToSpawn, Quaternion.identity);

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
