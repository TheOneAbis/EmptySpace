using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject player;
    private Animator doorAnimator;
    private float distToPlayer;
    private bool isOpen = false;

    public float openDistance = 4.0f;
    public bool locked = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorAnimator = GetComponent<Animator>();
        doorAnimator.Play("Base Layer.door_3_closed", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!locked)
        {
            // Open automatically when player is in range
            if (!isOpen && distToPlayer < openDistance)
            {
                Open();
            }
            else if (isOpen && distToPlayer > openDistance)
            {
                Close();
            }
        }
    }

    void Lock()
    {
        locked = true;
        Close();
    }

    void Open()
    {
        doorAnimator.Play("Base Layer.door_3_open", 0, 0);
        isOpen = true;
    }
    void Close()
    {
        doorAnimator.Play("Base Layer.door_3_close", 0, 0);
        isOpen = false;
    }
}
