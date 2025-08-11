using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private SpecialPowers spScript;
   
    [SerializeField] private Sprite Icon;

    private SurviveTime timer;

    void Start()
    {
        timer = FindAnyObjectByType<SurviveTime>();
        spScript = GameObject.FindWithTag("Player").GetComponent<SpecialPowers>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (timer != null)
            {
                timer.ResetNoPowerUpTimer();
            }

            foreach (var mission in MissionManager.Instance.currentMissions)
            {
                if (mission.missionType == Mission.MissionType.speed && !mission.isCompleted)
                {
                    mission.currentValue++;
                    if (mission.currentValue >= mission.targetValue)
                        mission.isCompleted = true;
                }
            }
            gameObject.SetActive(false);
            spScript.ActivateSpeed(10f,15f,Icon);
            
        }else if (collision.tag == "Missile")
        {
            Animator animator = collision.GetComponent<Animator>();
            animator.Play("Explosion");
            
            Destroy(collision.gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        }
    }
}
