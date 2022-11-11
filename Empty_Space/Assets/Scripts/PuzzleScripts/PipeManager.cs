using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PipeManager : MonoBehaviour
{
    public GameObject PipesHolder;
    public GameObject[] Pipes;
    //for win statement
    public GameObject door;
    public GameObject canvas;
    public GameObject player;
    public GameObject interactionManager;
    public bool thatOnePuzzle;

    [SerializeField]
    int totalPipes = 0;
    [SerializeField]
    int totalNeeded = 0;
    [SerializeField]
    int correctedPipes = 0;

    // Start is called before the first frame update
    public void Start()
    {
        totalPipes = PipesHolder.transform.childCount;

        Pipes = new GameObject[totalPipes];

        for(int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipesHolder.transform.GetChild(i).gameObject;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Complete();
    }

    // Update is called once per frame
    public void correctMove()
    {
        correctedPipes += 1;

        if(correctedPipes == totalNeeded)
            Complete();
    }

    public void wrongMove()
    {
        correctedPipes -= 1;
    }

    public void Complete()
    {
        canvas.SetActive(false);
        player.GetComponent<FirstPersonController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(thatOnePuzzle)
        {
            // Lock hallway door to block monster
            door.GetComponent<DoorController>().Lock();
            // Unlock doors to lvl 2
            for (int i = 1; i <= 3; i++)
                GameObject.Find($"DoorEnterLvl2_{i}").GetComponent<DoorController>().Unlock();
        }
        else
        {
            door.GetComponent<DoorController>().Unlock();
        }
        
        interactionManager.GetComponent<InteractScript>().inUI = false;
    }
}
