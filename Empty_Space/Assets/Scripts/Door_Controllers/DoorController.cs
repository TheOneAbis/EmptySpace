using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject player; // player ref
    private Animator doorAnimator; // this door's animation component
    private float distToPlayer; // player's distance from this door
    private bool isOpen; // is this door currently open? (not meant for public use; use 'locked' for that)
    private Light[] doorLights = new Light[2];
    private float randomInterval;
    private int clickForce;
    private bool interacted;
    private float jamCooldown;
    private UIManagement UIManager;
    private RadialProgressController clickDisplayer;

    [Tooltip("Minimum distance the player must be from the door for it to automatically open (if dynamic)")]
    public float openDistance = 4.0f;
    [Tooltip("Is this door interactable, or just decor?")]
    public bool interactable = true;
    [Tooltip("Is this door locked (will not open)?")]
    public bool locked = false;
    [Tooltip("When unlocked, does this door open and close automatically when the player is close?")]
    public bool dynamic = true;
    [Tooltip("Is this door jammed? (Requires player to interact with it directly to open)")]
    public bool jammed = false;
    [Tooltip("Name of this door's open animation (check the Animator component)")]
    public string openAnimationString;
    [Tooltip("Name of this door's close animation (check the Animator component)")]
    public string closeAnimationString;
    

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        // Set door bools properly based on what the developer put it as in Unity
        if (!interactable)
        {
            jammed = false;
            locked = true;
            dynamic = false;
            jammed = false;
        }
        else if (jammed)
        {
            dynamic = false;
            locked = false;

            // set up vars related to jammed door interaction
            randomInterval = 0.0f;
            UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
            clickDisplayer = GameObject.Find("RadialProgress").GetComponent<RadialProgressController>();
            clickForce = 0;
            interacted = false;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        doorLights[0] = transform.Find("Point Light").GetComponent<Light>();
        doorLights[1] = transform.Find("Point Light 2").GetComponent<Light>();
        
        doorAnimator = GetComponent<Animator>();

        // make sure door is closed when the game starts, unless it is an unlocked non-dynamic door
        if (jammed)
        {
            doorAnimator.Play($"Base Layer.{openAnimationString}", 0, 0.15f);
            doorAnimator.SetFloat("SpeedMultiplier", 0.0f);
        }
        else if (locked || dynamic) 
            doorAnimator.Play($"Base Layer.{closeAnimationString}", 0, 0);
        else
        {
            isOpen = true;
            doorAnimator.Play($"Base Layer.{openAnimationString}", 0, 0);
        } 


        if (interactable)
        {
            foreach (Light light in doorLights)
            {
                light.intensity = 0.5f;
                light.color = locked ? Color.red : Color.green;
            }
        }
        jamCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            // update player's distance from door
            distToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // Automatic opening/closing if dynamic and unlocked
            if (dynamic && !locked)
            {
                if (!isOpen && distToPlayer < openDistance) Open();
                else if (isOpen && distToPlayer > openDistance) Close();
            }

            // Funky Glitchy Lights if door is jammed
            if (jammed)
            {
                if (randomInterval > 0) 
                    randomInterval -= Time.deltaTime;
                else
                {
                    foreach (Light light in doorLights)
                    {
                        light.intensity = Random.Range(0.01f, 0.3f);
                        light.color = Random.Range(0, 2) == 0 ? Color.green : Color.red;
                    }
                    randomInterval = Random.Range(0.01f, 0.12f);
                }

                // Interact w/ door when within distance, and spam click to open it
                if (!isOpen && distToPlayer < openDistance)
                {
                    if (!interacted)
                    {
                        UIManager.DisplayTooltip(Tooltip.Interact);
                        if (Input.GetMouseButtonDown(0))
                        {
                            interacted = true;
                            player.GetComponent<FirstPersonController>().enabled = false;
                            StartCoroutine(FaceDoor());
                        }
                    }
                    else
                    {
                        // Click rapidly to force open the door
                        UIManager.DisplayCustomTooltip("[Left Click] Rapidly: Force Open Door");
                        jamCooldown -= Time.deltaTime;
                        if (Input.GetMouseButton(0) && jamCooldown <= 0)
                        {
                            clickForce++;
                            clickDisplayer.clickDisplay(clickForce, 10.0f);
                            jamCooldown = .15f;
                            if (clickForce < 10) StartCoroutine(IncreaseDoorOpen());
                            else
                            {
                                doorAnimator.SetFloat("SpeedMultiplier", 1.0f);
                                interacted = false;
                                Unlock();
                                jammed = false;
                                player.GetComponent<FirstPersonController>().enabled = true;
                                clickDisplayer.clickReset();
                            }
                        }
                    }
                }
            }
        }
    }

    // Lock this door.
    public void Lock()
    {
        if (interactable)
        {
            locked = true;
            foreach (Light light in doorLights) 
                light.color = Color.red;
            Close();
        }
    }

    // Unlock this door.
    public void Unlock()
    {
        if (interactable)
        {
            locked = false;
            foreach (Light light in doorLights)
                light.color = Color.green;
            if (!dynamic) Open();
        }
    }

    // Internal helper function; opens the door, playing its respective open animation
    public void Open()
    {
        doorAnimator.Play($"Base Layer.{openAnimationString}", 0, 0);
        isOpen = true;
    }
    // Internal helper function; closes the door, playing its respective close animation
    public void Close()
    {
        doorAnimator.Play($"Base Layer.{closeAnimationString}", 0, 0);
        isOpen = false;
    }

    public int ClickForce
    {
        get { return clickForce; }
        set { clickForce = value; }
    }

    // Resets this door to its initial state on startup.
    public void ResetState(bool interactable, bool jammed, bool locked, bool dynamic)
    {
        this.interactable = interactable;
        this.jammed = jammed;
        this.locked = locked;
        this.dynamic = dynamic;
        Start();
    }


    // -- USE THE BELOW COROUTINE FUNCTIONS ON JAMMED DOORS ONLY -- \\

    // Open the door a little bit on mouse click
    private IEnumerator IncreaseDoorOpen()
    {
        doorAnimator.SetFloat("SpeedMultiplier", 0.75f);
        yield return new WaitForSeconds(0.025f);
        doorAnimator.SetFloat("SpeedMultiplier", 0.0f);
    }

    // Face the door on interaction
    private IEnumerator FaceDoor()
    {
        float progress = 0, progressMax = 0.3f;
        Quaternion start = player.transform.rotation;
        Vector3 lookDirection = (transform.position - player.transform.position);
        lookDirection.y = 0;
        Quaternion end = Quaternion.LookRotation(lookDirection.normalized, transform.up);
        //Quaternion camStart = player.transform.GetChild(0).localRotation;
        while (progress < progressMax)
        {
            player.transform.rotation = Quaternion.Lerp(start, end, progress / progressMax);
            progress += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
