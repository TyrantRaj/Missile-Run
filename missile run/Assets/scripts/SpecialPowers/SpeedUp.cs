using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    //[SerializeField] private GameObject missile;
    private GameObject player;
    private PlayerMovement player_script;
    private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        //animator = missile.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        player_script = player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            player_script.speed = 10f;
            Invoke("SpeedUpp", speed);
        }else if (collision.tag == "Missile")
        {
            Animator animator = collision.GetComponent<Animator>();
            animator.Play("Explosion");
            
            Destroy(collision.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }


    private void SpeedUpp()
    {
        player_script.speed = 5;
        Destroy(gameObject);
    }


}
