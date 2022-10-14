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

    [SerializeField]
    int totalPipes = 0;
    [SerializeField]
    int totalNeeded = 0;
    [SerializeField]
    int correctedPipes = 0;

    // Start is called before the first frame update
    void Start()
    {
        totalPipes = PipesHolder.transform.childCount;

        Pipes = new GameObject[totalPipes];

        for(int i = 0; i < Pipes.Length; i++)
        {
            Pipes[i] = PipesHolder.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    public void correctMove()
    {
        correctedPipes += 1;

        if(correctedPipes == totalNeeded)
        {
            canvas.SetActive(false);
            player.GetComponent<FirstPersonController>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            door.GetComponent<DoorController>().Unlock();
        }
    }

    public void wrongMove()
    {
        correctedPipes -= 1;
    }
}