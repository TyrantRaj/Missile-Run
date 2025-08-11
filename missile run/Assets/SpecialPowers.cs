using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpecialPowers : MonoBehaviour
{
    // public - coin script reads this
    public bool Is2xCoin = false;

    [Header("Player")]
    public PlayerMovement movement; // assign (or let Start() GetComponent)

    [Header("UI (assign in inspector)")]
    public Transform powerUpListParent;    // the Vertical Layout Group container in Canvas
    public GameObject powerUpTimerPrefab;  // the prefab you made with PowerUpTimerUI

    [Header("Settings")]
    public bool mergeSamePowerUp = true;   // if true, re-picks refresh timer instead of adding duplicate row
    public float updateInterval = 0.1f;    // UI update frequency

    private List<PowerUpTimer> activePowerUps = new List<PowerUpTimer>();
    private float originalSpeed = 0f;

    private void Start()
    {
        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (movement != null)
            originalSpeed = movement.speed;

        if (powerUpListParent == null)
            Debug.LogWarning("SpecialPowers: assign powerUpListParent (UI container).");

        if (powerUpTimerPrefab == null)
            Debug.LogWarning("SpecialPowers: assign powerUpTimerPrefab (timer row prefab).");
    }

    // --- Public convenience activators ---

    // Speed: set boostedSpeed for `duration` seconds.
    public void ActivateSpeed(float boostedSpeed, float duration, Sprite icon = null)
    {
        string id = "Speed";
        bool alreadyActive = activePowerUps.Any(t => t.id == id);

        CreateOrRefreshPowerUp(id, duration, icon,
            onStart: (timer) =>
            {
                // apply speed only if no other speed power was active before this
                if (!alreadyActive && movement != null)
                    movement.speed = boostedSpeed;
            },
            onEnd: () =>
            {
                // when no more speed timers remain, revert
                if (!activePowerUps.Any(t => t.id == id) && movement != null)
                    movement.speed = originalSpeed;
            },
            displayName: "Speed");
    }

    // Double coin: enables Is2xCoin for `duration` seconds.
    public void ActivateDoubleCoin(float duration, Sprite icon = null)
    {
        string id = "2xCoins";
        bool alreadyActive = activePowerUps.Any(t => t.id == id);

        CreateOrRefreshPowerUp(id, duration, icon,
            onStart: (timer) =>
            {
                if (!alreadyActive)
                    Is2xCoin = true;
            },
            onEnd: () =>
            {
                if (!activePowerUps.Any(t => t.id == id))
                    Is2xCoin = false;
            },
            displayName: "2x Coins");
    }

    // --- Core: create or refresh a timer row ---
    private void CreateOrRefreshPowerUp(string id, float duration, Sprite icon, Action<PowerUpTimer> onStart, Action onEnd, string displayName = null)
    {
        // If an existing timer for this id exists and merging is enabled, refresh it.
        PowerUpTimer existing = activePowerUps.Find(t => t.id == id);
        if (existing != null && mergeSamePowerUp)
        {
            existing.remaining = duration;
            existing.ui.UpdateTimerText(existing.remaining);
            return;
        }

        bool alreadyActive = activePowerUps.Any(t => t.id == id);

        // instantiate UI row under the vertical layout group
        GameObject uiObj = Instantiate(powerUpTimerPrefab, powerUpListParent);
        uiObj.transform.SetParent(powerUpListParent, false); // ensure proper parenting
        uiObj.transform.SetSiblingIndex(0);                  // move to top


        PowerUpTimerUI ui = uiObj.GetComponent<PowerUpTimerUI>();
        if (ui != null)
        {
            ui.SetIcon(icon);
            //if (!string.IsNullOrEmpty(displayName)) ui.SetName(displayName);
            ui.UpdateTimerText(duration);
        }

        PowerUpTimer timer = new PowerUpTimer()
        {
            id = id,
            duration = duration,
            remaining = duration,
            ui = ui,
            uiObject = uiObj,
            onEnd = onEnd
        };

        activePowerUps.Add(timer);

        if (!alreadyActive)
            onStart?.Invoke(timer);

        timer.coroutine = StartCoroutine(RunTimer(timer));
    }

    private IEnumerator RunTimer(PowerUpTimer t)
    {
        while (t.remaining > 0f)
        {
            if (t.ui != null)
                t.ui.UpdateTimerText(t.remaining);

            yield return new WaitForSeconds(updateInterval);
            t.remaining -= updateInterval;
        }

        // finished
        activePowerUps.Remove(t);

        if (t.uiObject != null)
            Destroy(t.uiObject);

        t.onEnd?.Invoke();
    }

    // Inside SpecialPowers.cs
    public void ActivateMagnet(float newPickupDistance, float duration, Sprite icon = null)
    {
        string id = "Magnet";
        bool alreadyActive = activePowerUps.Any(t => t.id == id);

        CreateOrRefreshPowerUp(id, duration, icon,
            onStart: (timer) =>
            {
                if (!alreadyActive && movement != null)
                    movement.CoinpickupDistance = newPickupDistance;
            },
            onEnd: () =>
            {
                if (!activePowerUps.Any(t => t.id == id) && movement != null)
                    movement.CoinpickupDistance = 2f; // default value
            },
            displayName: "Magnet");
    }


    // simple container for each running timer
    private class PowerUpTimer
    {
        public string id;
        public float duration;
        public float remaining;
        public PowerUpTimerUI ui;
        public GameObject uiObject;
        public Coroutine coroutine;
        public Action onEnd;
    }
}
