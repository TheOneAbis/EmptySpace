using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;


public class InteractScript : MonoBehaviour
{
    public float timer;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    public GameObject canvas4;
    public GameObject[] inactivePipes1;
    public GameObject[] inactivePipes2;
    public GameObject player;
    [SerializeField]
    public bool inUI = false;
    private bool lookingAtPuzzle = false;

    public GameObject battery1;
    public GameObject battery2;
    public GameObject battery3;
    public GameObject battery4;
    public GameObject battery1Holder;
    public GameObject battery2Holder;
    public GameObject battery3Holder;
    public GameObject battery4Holder;

    private UIManagement UIManager;


    public float raycastDistance = 3; //Adjust to suit your use case

    public Text interactText; //Create GUI Canvas on your scene if you havnt already and a UI Text Element in a suitable location on your screen and apply it to this Text variable

    private void Start()
    {
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // This creates a 'ray' at the Main Camera's Centre Point essentially the centre of the users Screen

        RaycastHit hit; //This creates a Hit which is used to callback the object that was hit by the Raycast

        if (Physics.Raycast(ray, out hit, raycastDistance)) //Actively creates a ray using the above set perameters at the predeterminded distance
        {
            //Item Raycast Detection
            if (hit.collider.CompareTag("puzzleOne"))
            {
                if (!lookingAtPuzzle)
                {
                    UIManager.DisplayTooltip(Tooltip.Interact);
                    lookingAtPuzzle = true;
                }
                //interactText.text = "Press [E] to interact"; //Setting the Interaction Text to let the player know they are now hovering an interactable object
                if (Input.GetKeyDown(KeyCode.E))//Check if the player has pressed the Interaction button
                {

                    if (!inUI)
                    {
                        canvas1.SetActive(true);
                        player.GetComponent<FirstPersonController>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        inUI = true;
                    }
                }
            }
            else if (hit.collider.CompareTag("puzzleTwo"))
            {
                if (!lookingAtPuzzle)
                {
                    UIManager.DisplayTooltip(Tooltip.Interact);
                    lookingAtPuzzle = true;
                }
                //interactText.text = "Press [E] to interact"; //Setting the Interaction Text to let the player know they are now hovering an interactable object
                if (Input.GetKeyDown(KeyCode.E))//Check if the player has pressed the Interaction button
                {
                    if (!inUI)
                    {
                        canvas2.SetActive(true);
                        for (int i = 0; i < inactivePipes1.Length; i++)
                        {
                            inactivePipes1[i].SetActive(false);
                        }
                        
                        if(player.GetComponent<PlayerInventoryManager>().GetAmount("Battery") == 2)
                        {
                            battery1.SetActive(true);
                            battery2.SetActive(true);
                        }
                        else if (player.GetComponent<PlayerInventoryManager>().GetAmount("Battery") == 1)
                        {
                            battery1.SetActive(true);
                            battery2.SetActive(false);
                        }
                        else
                        {
                            battery1.SetActive(false);
                            battery2.SetActive(false);
                        }
                        
                        player.GetComponent<FirstPersonController>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        inUI = true;
                    }
                }
            }
            else if (hit.collider.CompareTag("puzzleThree"))//If nothing at all with an above tag was hit with the Raycast within the specified distance then run this
            {
                if (!lookingAtPuzzle)
                {
                    UIManager.DisplayTooltip(Tooltip.Interact);
                    lookingAtPuzzle = true;
                }
                //interactText.text = "Press [E] to interact"; //Setting the Interaction Text to let the player know they are now hovering an interactable object
                if (Input.GetKeyDown(KeyCode.E))//Check if the player has pressed the Interaction button
                {
                    if (!inUI)
                    {
                        canvas3.SetActive(true);
                        for (int i = 0; i < inactivePipes2.Length; i++)
                        {
                            inactivePipes2[i].SetActive(false);
                        }

                        if (player.GetComponent<PlayerInventoryManager>().GetAmount("Battery") == 4)
                        {
                            battery3.SetActive(true);
                            battery4.SetActive(true);
                        }
                        else if (player.GetComponent<PlayerInventoryManager>().GetAmount("Battery") == 3)
                        {
                            battery3.SetActive(true);
                            battery4.SetActive(false);
                        }
                        else
                        {
                            battery3.SetActive(false);
                            battery4.SetActive(false);
                        }

                        player.GetComponent<FirstPersonController>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        inUI = true;
                    }
                }
            }
            else if (hit.collider.CompareTag("puzzleFour"))
            {
                if (!lookingAtPuzzle)
                {
                    UIManager.DisplayTooltip(Tooltip.Interact);
                    lookingAtPuzzle = true;
                }
                //interactText.text = "Press [E] to interact"; //Setting the Interaction Text to let the player know they are now hovering an interactable object
                if (Input.GetKeyDown(KeyCode.E))//Check if the player has pressed the Interaction button
                {

                    if (!inUI)
                    {
                        canvas4.SetActive(true);
                        player.GetComponent<FirstPersonController>().enabled = false;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        inUI = true;
                    }
                }
            }
            else
            {
                if (lookingAtPuzzle)
                {
                    UIManager.DisplayTooltip(Tooltip.None);
                    lookingAtPuzzle = false;
                }
            }
        }
        
        if (inUI && Input.GetKey(KeyCode.Escape))
        {
            canvas1.SetActive(false);
            canvas2.SetActive(false);
            canvas3.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inUI = false;
        }

        //for battery stuff
        if (battery1Holder.GetComponent<BatteryDrop>().placed == true)
        {
            for (int i = 0; i < inactivePipes1.Length - 4; i++)
            {
                inactivePipes1[i].SetActive(true);
            }
        }
        if(battery2Holder.GetComponent<BatteryDrop>().placed == true)
        {
            for (int i = inactivePipes1.Length - 4; i < inactivePipes1.Length; i++)
            {
                inactivePipes1[i].SetActive(true);
            }
        }
        if(battery3Holder.GetComponent<BatteryDrop>().placed == true)
        {
            for (int i = 0; i < inactivePipes2.Length - 5; i++)
            {
                inactivePipes2[i].SetActive(true);
            }
        }
        if(battery4Holder.GetComponent<BatteryDrop>().placed == true)
        {
            for (int i = inactivePipes2.Length - 5; i < inactivePipes2.Length; i++)
            {
                inactivePipes2[i].SetActive(true);
            }
        }
    }
}
