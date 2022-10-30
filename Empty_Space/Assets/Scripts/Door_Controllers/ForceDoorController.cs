using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDoorController : MonoBehaviour
{
    private GameObject player;
    private Animator doorAnimator; // this door's animation component
    private float distToPlayer; // player's distance from this door
    private bool isOpen = false; // is this door currently open? (not meant for public use; use 'locked' for that)
    private Light doorLightFront, doorLightBack;
    [Tooltip("Name of this door's open animation (check the Animator component)")]
    public string openAnimationString;
    [Tooltip("Name of this door's close animation (check the Animator component)")]
    public string closeAnimationString;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorLightFront = transform.Find("Point Light").GetComponent<Light>();
        doorLightBack = transform.Find("Point Light 2").GetComponent<Light>();
        doorAnimator = GetComponent<Animator>();
        doorAnimator.Play($"Base Layer.{closeAnimationString}", 0, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
