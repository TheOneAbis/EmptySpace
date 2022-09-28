using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public float timer;
    public GameObject item;

    void OnTriggerStay(Collider col)
    {
        if(Input.GetKey (KeyCode.E))
        {
            Debug.Log("Interacted");
            item.SetActive(false);
        }
    }
}
