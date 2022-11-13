using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameObject player;
    [Tooltip("How long (in seconds) to hold down the Interact key before picking up this collectible.")]
    public float keyHoldTime = 0.0f;
    private float keyHoldTimeLeft;
    private UIManagement UIManager;
    private bool showInteract;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        keyHoldTimeLeft = keyHoldTime;
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
        showInteract = false;
    }

    private void Update()
    {
        if (showInteract) UIManager.DisplayTooltip(Tooltip.Interact);
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
                if (keyHoldTimeLeft <= 0)
                {
                    player.GetComponent<PlayerInventoryManager>().AddToInventory(gameObject);
                    showInteract = false;
                }
            }
            else keyHoldTimeLeft = keyHoldTime; // if not holding down (or released), reset timer
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") showInteract = false;
    }
}
