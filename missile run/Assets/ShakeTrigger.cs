using UnityEngine;
using Cinemachine;

public class ShakeTrigger : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void TriggerShake()
    {
        impulseSource.GenerateImpulse();
    }
}
