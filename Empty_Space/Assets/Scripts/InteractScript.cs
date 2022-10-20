using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;


public class InteractScript : MonoBehaviour
{
    public float timer;
    public GameObject canvas;
    public GameObject player;
    private bool inUI = false;

    void OnTriggerStay(Collider col)
    {
        if(Input.GetKey(KeyCode.E))
        {
            if(inUI)
            {
                
            }
            else
            {
                OpenUI();
                inUI = true;
            }
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            if(inUI)
            {
                CloseUI();
                inUI = false;
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

    public float raycastDistance = 3; //Adjust to suit your use case

    public Text interactText; //Create GUI Canvas on your scene if you havnt already and a UI Text Element in a suitable location on your screen and apply it to this Text variable

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // This creates a 'ray' at the Main Camera's Centre Point essentially the centre of the users Screen

        RaycastHit hit; //This creates a Hit which is used to callback the object that was hit by the Raycast

        if (Physics.Raycast(ray, out hit, raycastDistance)) //Actively creates a ray using the above set perameters at the predeterminded distance
        {
            //Item Raycast Detection
            if (hit.collider.CompareTag("interactableObject"))//Checking if the Raycast has hit a collider with the tag of note
            {
                interactText.text = "Press [E] to interact"; //Setting the Interaction Text to let the player know they are now hovering an interactable object

                if (Input.GetKeyDown(KeyCode.E))//Check if the player has pressed the Interaction button
                {
                    //add stuff to do
                }
            }
            /*
            else if (hit.collider.CompareTag("NEW_OBJECT_TAG_HERE"))
            {
                //Add a different object here if you choose to want more objects to be able to be detected by the Raycast such as doors etc
            }
            */
            else //If nothing at all with an above tag was hit with the Raycast within the specified distance then run this
            {
                if (interactText.text != "")//If the interactText is not already set as nothing then set it to nothing - this is to help optimise and save from constantly spamming this request
                {
                    interactText.text = ""; //Removing the text as nothing was detected by the raycast
                }
            }
        }
    }
}
