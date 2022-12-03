using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Collectible : MonoBehaviour
{
    private GameObject player;
    [Tooltip("How long (in seconds) to hold down the Interact key before picking up this collectible.")]
    public float keyHoldTime = 0.0f;
    private float keyHoldTimeLeft;
    private UIManagement UIManager;
    private RadialProgressController clickDisplayer;
    private bool showInteract;
    public bool gravityBattery = false;
    public bool collected;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        keyHoldTimeLeft = keyHoldTime;
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
        clickDisplayer = GameObject.Find("RadialProgress").GetComponent<RadialProgressController>();
        showInteract = false;
        collected = false;
    }

    private void Update()
    {
        if (showInteract) UIManager.DisplayCustomTooltip("[Hold Left Click] Pick Up Battery");
    }

    // Player comes within collectible radius
    private void OnTriggerStay(Collider other)
    {
        if (!collected && other.tag == "Player")
        {
            showInteract = true;
            // Player presses (or holds depending on keyHoldTime setting) interaction key, collect this object
            if (Input.GetMouseButton(0))
            {
                keyHoldTimeLeft -= Time.deltaTime;
                clickDisplayer.clickDisplay(keyHoldTime - keyHoldTimeLeft, keyHoldTime);
                if (keyHoldTimeLeft <= 0)
                {
                    Collect();
                }
            }
            else
            {
                keyHoldTimeLeft = keyHoldTime; // if not holding down (or released), reset timer
                clickDisplayer.clickReset();
            }
        }
    }

    public void Collect()
    {
        collected = true;
        showInteract = false;
        clickDisplayer.clickReset();

        // Activate Zero Gravity (this is only for the battery in the Artificial Gravity room)
        if (gravityBattery)
        {
            player.GetComponent<FirstPersonController>().Gravity = 0;
            player.GetComponent<FirstPersonController>().zeroG = true;
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<MonsterChaseController>().canKill = true;

            UIManager.QueueDialogue("Vivy:", "WARNING: Artificial Gravity Generator has been disabled.", 0, 6);
            UIManager.QueueDialogue("Space Boi:", "This battery must have been powering it. Fortunately, I can use my suit's thruster to get around.", 0.25f, 7);
            UIManager.DisplayCustomTooltip("[Shift] to use Thruster", 13, 6);
        }
        player.GetComponent<PlayerInventoryManager>().AddToInventory(gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") showInteract = false;
    }
}
