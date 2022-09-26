using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public float timer;
    public GameObject item;

    void OnTriggerStay(Collider col)
    {
        if(Input.GetKeyDown (KeyCode.E))
        {
            Debug.Log("Interacted");
            item.SetActive(false);
        }
    }
}
