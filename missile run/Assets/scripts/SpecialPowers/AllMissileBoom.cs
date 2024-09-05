using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMissileBoom : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject[] go = GameObject.FindGameObjectsWithTag("Missile");
            foreach (GameObject g in go)
                dest(g);

            Destroy(gameObject);
        }
        else if (collision.tag == "Missile")
        {
            Animator animator = collision.GetComponent<Animator>();
            animator.Play("Explosion");

            Destroy(collision.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }

    void dest(GameObject gameobj)
    {
        targeting_missile player_script = gameobj.GetComponent<targeting_missile>();    
        player_script.missile_speed = 0;
        player_script.rotate_speed = 0;
        Animator animator = gameobj.GetComponent<Animator>();
        animator.Play("Explosion");
        Destroy(gameobj, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
