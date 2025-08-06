using UnityEngine;
using System;

public class MissileBase : MonoBehaviour
{
    public event Action OnMissileDestroyed;

    private void OnDestroy()
    {
        OnMissileDestroyed?.Invoke();
    }
}
