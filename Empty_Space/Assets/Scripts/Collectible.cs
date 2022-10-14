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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        keyHoldTimeLeft = keyHoldTime;
    }

    // Player comes within collectible radius
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            // Player presses (or holds depending on keyHoldTime setting) interaction key, collect this object
            if (Input.GetKey(KeyCode.E))
            {
                keyHoldTimeLeft -= Time.deltaTime;
                if (keyHoldTimeLeft <= 0) player.GetComponent<PlayerInventoryManager>().AddToInventory(gameObject);
            }
            else keyHoldTimeLeft = keyHoldTime; // if not holding down (or released), reset timer
        }
    }
}