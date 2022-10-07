using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Player comes within collectible radius
    private void OnTriggerStay(Collider other)
    {
        // Player presses interaction key, collect this object
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<PlayerInventoryManager>().AddToInventory(gameObject);
        }
    }
}
