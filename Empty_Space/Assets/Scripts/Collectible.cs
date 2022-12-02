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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        keyHoldTimeLeft = keyHoldTime;
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
        clickDisplayer = GameObject.Find("RadialProgress").GetComponent<RadialProgressController>();
        showInteract = false;
    }

    private void Update()
    {
        if (showInteract) UIManager.DisplayCustomTooltip("[Hold Left Click] Pick Up Battery");
    }

    // Player comes within collectible radius
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            showInteract = true;
            // Player presses (or holds depending on keyHoldTime setting) interaction key, collect this object
            if (Input.GetMouseButton(0))
            {
                keyHoldTimeLeft -= Time.deltaTime;
                clickDisplayer.clickDisplay(keyHoldTime - keyHoldTimeLeft, keyHoldTime);
                if (keyHoldTimeLeft <= 0)
                {
                    player.GetComponent<PlayerInventoryManager>().AddToInventory(gameObject);
                    showInteract = false;
                    clickDisplayer.clickReset();
                    if (gravityBattery)
                    {
                        player.GetComponent<FirstPersonController>().Gravity = 0;
                        player.GetComponent<FirstPersonController>().zeroG = true;
                    }
                }
            }
            else
            {
                keyHoldTimeLeft = keyHoldTime; // if not holding down (or released), reset timer
                clickDisplayer.clickReset();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") showInteract = false;
    }
}
