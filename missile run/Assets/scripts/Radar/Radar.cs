using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private Transform pfRadarPing;

    private Transform sweepTransform;
    private float rotatingSpeed;
    private float radarDistance;
    private List<Collider2D> colliderlist;

    private void Awake()
    {
        sweepTransform = transform.Find("Sweep");
        rotatingSpeed = 360f;
        radarDistance = 50;
        colliderlist = new List<Collider2D>();
    }

    private void Update()
    {
        float previousRotation = (sweepTransform.eulerAngles.z % 360) - 180;
        sweepTransform.eulerAngles -= new Vector3(0, 0, rotatingSpeed * Time.deltaTime);
        float currentRotation = (sweepTransform.eulerAngles.z % 360) - 180;

        if (previousRotation <= 0 && currentRotation >= 0) {
            colliderlist.Clear();
        }

        RaycastHit2D[] raycastHit2DArray = Physics2D.RaycastAll(transform.position, GetVectorFromAngle(sweepTransform.eulerAngles.z), radarDistance);
        foreach(RaycastHit2D raycastHit2D in raycastHit2DArray)
        {
            if (raycastHit2D.collider != null && raycastHit2D.collider.gameObject.GetComponent<PlayerMovement>() == null && raycastHit2D.collider.gameObject.GetComponent<FireScript>() == null)
            {

                if (!colliderlist.Contains(raycastHit2D.collider))
                {
                    colliderlist.Add(raycastHit2D.collider);
                    RadarPing radarPing = Instantiate(pfRadarPing, raycastHit2D.point, Quaternion.identity).GetComponent<RadarPing>();

                    if (raycastHit2D.collider.gameObject.GetComponent<targeting_missile>() != null)
                    {
                        radarPing.SetColor(new Color(1, 0, 0));
                    }

                    if (raycastHit2D.collider.gameObject.GetComponent<AllMissileBoom>() != null || raycastHit2D.collider.gameObject.GetComponent<SlowMotion>() != null || raycastHit2D.collider.gameObject.GetComponent<SpeedUp>() != null)
                    {
                        radarPing.SetColor(new Color(0, 1, 0));
                    }
                }

            }
        }
       
    }

    private static Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }
}
