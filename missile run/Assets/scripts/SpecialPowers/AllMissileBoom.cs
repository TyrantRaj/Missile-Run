using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMissileBoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
            foreach (GameObject missile in missiles)
            {
                HandleMissileExplosion(missile);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Missile"))
        {
            HandleMissileExplosion(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void HandleMissileExplosion(GameObject missile)
    {
        // Stop movement (if any)
        if (missile.TryGetComponent<targeting_missile>(out var targeting))
        {
            targeting.missile_speed = 0;
            targeting.rotate_speed = 0;
        }
        else if (missile.TryGetComponent<WaveMissile>(out var wave))
        {
            wave.missile_speed = 0;
            wave.enabled = false; // Disable movement script
        }
        

        // Trigger animation if available
        if (missile.TryGetComponent<Animator>(out var anim))
        {
            anim.Play("Explosion");
            Destroy(missile, anim.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Destroy(missile);
        }

        // Optional: Remove from indicator system
        if (FindObjectOfType<IndicatorManager>() is { } indicatorMgr)
        {
            indicatorMgr.RemoveTarget(missile);
        }
    }
}
