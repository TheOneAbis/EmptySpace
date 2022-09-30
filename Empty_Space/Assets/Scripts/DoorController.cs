using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject player; // player ref
    private Animator doorAnimator; // this door's animation component
    private float distToPlayer; // player's distance from this door
    private bool isOpen = false; // is this door currently open? (not meant for public use; use 'locked' for that)

    [Tooltip("Minimum distance the player must be from the door for it to automatically open (if dynamic)")]
    public float openDistance = 4.0f;
    [Tooltip("Is this door locked (will not open)?")]
    public bool locked = false;
    [Tooltip("When unlocked, does this door open and close automatically when the player is close?")]
    public bool dynamic = true;
    [Tooltip("Name of this door's open animation (check the Animator component)")]
    public string openAnimationString;
    [Tooltip("Name of this door's close animation (check the Animator component)")]
    public string closeAnimationString;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorAnimator = GetComponent<Animator>();
        // make sure door is closed when the game starts, unless it is an unlocked non-dynamic door
        if (locked || dynamic) doorAnimator.Play($"Base Layer.{closeAnimationString}", 0, 0);
        else doorAnimator.Play($"Base Layer.{openAnimationString}", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // update player's distance from door
        distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Automatic opening/closing if dynamic and unlocked
        if (!locked && dynamic)
        {
            if (!isOpen && distToPlayer < openDistance) Open();
            else if (isOpen && distToPlayer > openDistance) Close();
        }
    }

    // Lock this door.
    void Lock()
    {
        locked = true;
        Close();
    }

    // Unlock this door.
    public void Unlock()
    {
        locked = false;
        if (!dynamic) Open();
    }

    // Internal helper function; opens the door, playing its respective open animation
    private void Open()
    {
        doorAnimator.Play($"Base Layer.{openAnimationString}", 0, 0);
        isOpen = true;
    }
    // Internal helper function; closes the door, playing its respective close animation
    private void Close()
    {
        doorAnimator.Play($"Base Layer.{closeAnimationString}", 0, 0);
        isOpen = false;
    }
    public void Load()
    {
        Start();
    }
}
