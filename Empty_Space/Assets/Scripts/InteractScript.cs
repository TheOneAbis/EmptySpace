using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class InteractScript : MonoBehaviour
{
    public float timer;
    public GameObject item;
    public GameObject canvas;
    public GameObject player;

    void OnTriggerStay(Collider col)
    {
        if(Input.GetKey (KeyCode.E))
        {
            Debug.Log("Interacted");
            item.SetActive(false);
            //canvas.SetActive(true);
            player.GetComponent<FirstPersonController>().enabled = false;
        }
    }
}
