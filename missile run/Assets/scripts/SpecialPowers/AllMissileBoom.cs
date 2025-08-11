using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AllMissileBoom : MonoBehaviour
{
    private SurviveTime timer;

    private void Start()
    {
        timer = FindAnyObjectByType<SurviveTime>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            foreach (var mission in MissionManager.Instance.currentMissions)
            {
                if (mission.missionType == Mission.MissionType.emf && !mission.isCompleted)
                {
                    mission.currentValue++;
                    if (mission.currentValue >= mission.targetValue)
                        mission.isCompleted = true;
                }
            }

            if(timer != null)
            {
                timer.ResetNoPowerUpTimer();
            }
            

            GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
            foreach (GameObject missile in missiles)
            {
                HandleMissileExplosion(missile);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Missile"))
        {
            return;
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
        else if (missile.TryGetComponent<OneHitMissile>(out var oneHit))
        {
            oneHit.missile_speed = 0;
            oneHit.rotate_speed = 0;
            oneHit.enabled = false; // Disable movement script
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
