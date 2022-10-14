using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

public class Cryo_Pod_Script : MonoBehaviour
{
    private GameObject player;
    private float playerSpeed, gravity;
    private GameObject[] switches;
    private bool canExit;

    private bool escaped = false;

    public float progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        switches = GameObject.FindGameObjectsWithTag("ExitLight");
        player = GameObject.FindGameObjectWithTag("Player");

        // temporarily store these value for re-enabling movement
        playerSpeed = player.GetComponent<FirstPersonController>().MoveSpeed;
        gravity = player.GetComponent<FirstPersonController>().Gravity;

        // disable player physics, essentially
        player.GetComponent<FirstPersonController>().MoveSpeed = 0;
        player.GetComponent<FirstPersonController>().Gravity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Player can exit if all lights are activated
        canExit = true;
        foreach (GameObject s in switches)
            if (!s.GetComponent<ExitLightController>().Activated) 
                canExit = false;

        // Interact to open the cryo pod and get out; can only do this once (obviously)
        if (canExit && !escaped)
        {
            escaped = true;
            foreach (GameObject s in switches) s.SetActive(false);
            StartCoroutine(leavePodAnimSequence()); // animation sequence
        }
    }

    IEnumerator leavePodAnimSequence()
    {
        GetComponentInChildren<PlayableDirector>().Play();
        yield return new WaitForSeconds(1);

        player.GetComponent<FirstPersonController>().enabled = false;

        float progress = 0, progressMax = 0.3f;
        Quaternion start = player.transform.rotation;
        Quaternion camStart = player.transform.GetChild(0).localRotation;
        while (progress < progressMax)
        {
            player.transform.rotation = Quaternion.Lerp(start, Quaternion.Euler(-20, 90, 0), progress / progressMax);
            player.transform.GetChild(0).localRotation = Quaternion.Lerp(camStart, Quaternion.Euler(0, 0, 0), progress / progressMax);
            progress += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        player.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(2);
        player.GetComponent<FirstPersonController>().MoveSpeed = playerSpeed;
        player.GetComponent<FirstPersonController>().Gravity = gravity;
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<FirstPersonController>().UpdateCinemachineTargetPitch();
        player.GetComponent<AudioSource>().Play(); // begin ambient music loop
    }
}
