using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class InteractScript : MonoBehaviour
{
    public float timer;
    public GameObject canvas;
    public GameObject player;
    private bool inUI = false;

    void OnTriggerStay(Collider col)
    {
        if(Input.GetKey (KeyCode.E))
        {
            if(inUI)
            {
                //CloseUI();
            }
            else
            {
                OpenUI();
            }
        }
    }


    public void CloseUI()
    {
        canvas.SetActive(false);
        player.GetComponent<FirstPersonController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inUI = false;
    }

    public void OpenUI()
    {
        canvas.SetActive(true);
        player.GetComponent<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inUI = true;
    }
}
