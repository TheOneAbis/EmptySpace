using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PuzzleUIManager : MonoBehaviour
{
    //ungabunga code
    public GameObject R1B1;
    public GameObject R1B2;
    public GameObject R1B3;
    private int R1B3state = 1;
    public GameObject R1B4;
    private int R1B4state = 1;
    public GameObject R2B1;
    private int R2B1state = 1;
    public GameObject R2B2;
    private int R2B2state = 1;
    public GameObject R2B3;
    private int R2B3state = 1;
    public GameObject R2B4;
    public GameObject R3B1;
    private int R3B1state = 1;
    public GameObject R3B2;
    private int R3B2state = 1;
    public GameObject R3B3;
    private int R3B3state = 1;
    public GameObject R3B4;
    private int R3B4state = 1;
    public GameObject R4B1;
    private int R4B1state = 1;
    public GameObject R4B2;
    private int R4B2state = 1;
    public GameObject R4B3;
    private int R4B3state = 1;
    public GameObject R4B4;
    private int R4B4state = 1;

    //game objs for script
    public GameObject interactable;
    public GameObject canvas;
    public GameObject player;
    public GameObject door;
    private bool complete = false;

    void Update()
    {
        //ungabunga check statement
        if(!complete && R1B3state == 4 && R1B4state == 3 && R2B1state == 3 && R2B2state == 4 && (R2B3state % 2) == 0 && (R3B1state % 2) == 0 && (R3B2state % 2) == 1 && R3B3state == 1 && R3B4state == 4 && R4B1state == 2 && R4B2state == 1 && (R4B3state % 2) == 0 && R4B4state == 4)
        {
            complete = true;
            Debug.Log("Finish");
            canvas.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            door.GetComponent<DoorController>().Unlock();
        }
    }

    //no state
    public void ButtonPushR1B1()
    {
        R1B1.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    //no state
    public void ButtonPushR1B2()
    {
        R1B2.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR1B3()
    {
        R1B3state++;
        if(R1B3state > 4)
        {
            R1B3state = 1;
        }
        R1B3.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR1B4()
    {
        R1B4state++;
        if (R1B4state > 4)
        {
            R1B4state = 1;
        }
        R1B4.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR2B1()
    {
        R2B1state++;
        if (R2B1state > 4)
        {
            R2B1state = 1;
        }
        R2B1.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR2B2()
    {
        R2B2state++;
        if (R2B2state > 4)
        {
            R2B2state = 1;
        }
        R2B2.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR2B3()
    {
        R2B3state++;
        if (R2B3state > 4)
        {
            R2B3state = 1;
        }
        R2B3.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    //no state
    public void ButtonPushR2B4()
    {
        R2B4.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR3B1()
    {
        R3B1state++;
        if (R3B1state > 4)
        {
            R3B1state = 1;
        }
        R3B1.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR3B2()
    {
        R3B2state++;
        if (R3B2state > 4)
        {
            R3B2state = 1;
        }
        R3B2.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR3B3()
    {
        R3B3state++;
        if (R3B3state > 4)
        {
            R3B3state = 1;
        }
        R3B3.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR3B4()
    {
        R3B4state++;
        if (R3B4state > 4)
        {
            R3B4state = 1;
        }
        R3B4.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR4B1()
    {
        R4B1state++;
        if (R4B1state > 4)
        {
            R4B1state = 1;
        }
        R4B1.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR4B2()
    {
        R4B2state++;
        if (R4B2state > 4)
        {
            R4B2state = 1;
        }
        R4B2.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR4B3()
    {
        R4B3state++;
        if (R4B3state > 4)
        {
            R4B3state = 1;
        }
        R4B3.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    public void ButtonPushR4B4()
    {
        R4B4state++;
        if (R4B4state > 4)
        {
            R4B4state = 1;
        }
        R4B4.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
    }

    //TODO
    public void Reset()
    {

    }
}
