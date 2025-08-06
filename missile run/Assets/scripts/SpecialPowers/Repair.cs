using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().Repair();
            Destroy(gameObject);
        }
        else if (collision.tag == "Missile")
        {
            return;
            Animator animator = collision.GetComponent<Animator>();
            animator.Play("Explosion");

            Destroy(collision.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }
}
