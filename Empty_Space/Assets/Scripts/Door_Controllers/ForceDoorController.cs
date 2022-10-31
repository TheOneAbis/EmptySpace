using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceDoorController : MonoBehaviour
{
    private GameObject player;
    private Animator doorAnimator; // this door's animation component
    private float distToPlayer; // player's distance from this door
    private bool isOpen = false; // is this door currently open? (not meant for public use; use 'locked' for that)
    [Tooltip("Name of this door's open animation (check the Animator component)")]
    public string openAnimationString;
    [Tooltip("Name of this door's close animation (check the Animator component)")]
    public string closeAnimationString;

    private Light[] doorLights = new Light[2];
    private float randomInterval;
    private UIManagement UIManager;
    private bool interacted = false;
    private int clickForce;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorLights[0] = transform.Find("Point Light").GetComponent<Light>();
        doorLights[1] = transform.Find("Point Light 2").GetComponent<Light>();
        randomInterval = 0.0f;
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
        clickForce = 0;

        doorAnimator = GetComponent<Animator>();
        doorAnimator.Play($"Base Layer.{openAnimationString}", 0, 0.15f);
        doorAnimator.SetFloat("SpeedMultiplier", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Funky Glitchy Lights to show the door is jammed/broken
        if (randomInterval > 0) randomInterval -= Time.deltaTime;
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
        if (Vector3.Distance(player.transform.position, transform.position) < 3.0f && !isOpen)
        {
            if (!interacted)
            {
                UIManager.DisplayTooltip(Tooltip.Interact);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interacted = true;
                    player.GetComponent<FirstPersonController>().enabled = false;
                    StartCoroutine(FaceDoor());
                }
            }
            else
            {
                // Stop interacting w/ door
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    interacted = false;
                    player.GetComponent<FirstPersonController>().enabled = true;
                }

                // Click rapidly to force open the door
                UIManager.DisplayTooltip(Tooltip.LeftClick);
                if (Input.GetMouseButtonDown(0))
                {
                    clickForce++;
                    if (clickForce < 10) StartCoroutine(IncreaseDoorOpen());
                    else
                    {
                        doorAnimator.SetFloat("SpeedMultiplier", 1.0f);
                        interacted = false;
                        isOpen = true;
                        player.GetComponent<FirstPersonController>().enabled = true;
                    }
                }
            }
        }
    }

    // Open the door a little bit on mouse click
    IEnumerator IncreaseDoorOpen()
    {
        doorAnimator.SetFloat("SpeedMultiplier", 0.75f);
        yield return new WaitForSeconds(0.025f);
        doorAnimator.SetFloat("SpeedMultiplier", 0.0f);
    }

    // Face the door on interaction
    IEnumerator FaceDoor()
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
