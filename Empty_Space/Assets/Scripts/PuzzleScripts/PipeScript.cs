using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{

    public float correctRotation;
    public bool isStraightPiece = false;
    public bool thatOneMessedUpPiece = false;
    public bool ignoreThis = false;
    [SerializeField]
    private bool isPlaced = false;

    GameObject puzzlemanager;

    private void Awake()
    {
        puzzlemanager = GameObject.Find("PuzzleManager");
    }

    void Start()
    {
        if(ignoreThis)
        {
            isPlaced = false;
        }
        else if (transform.eulerAngles.z == correctRotation || (transform.eulerAngles.z == correctRotation + 180 && isStraightPiece == true) && isPlaced == false)
        {
            isPlaced = true;
            puzzlemanager.GetComponent<PipeManager>().correctMove();
        }
    }


    public void ButtonPush()
    {
        //rotate pipe
        transform.Rotate(new Vector3(0f, 0f, 90f));

        //check to see if its in the correct rotation or not
        if(ignoreThis)
        {
            isPlaced = false;
        }
        else if(transform.eulerAngles.z == correctRotation || (transform.eulerAngles.z == correctRotation + 180 && isStraightPiece == true) && isPlaced == false )
        {
            isPlaced = true;
            puzzlemanager.GetComponent<PipeManager>().correctMove();
        }
        else if(transform.eulerAngles.z == -90 && isPlaced == false && thatOneMessedUpPiece == true)
        {
            isPlaced = true;
            puzzlemanager.GetComponent<PipeManager>().correctMove();
        }
        else if(isPlaced == true)
        {
            isPlaced = false;
            puzzlemanager.GetComponent<PipeManager>().wrongMove();
        }
    }
}
