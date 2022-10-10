using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Cryo_Pod_Script : MonoBehaviour
{
    private GameObject player;
    private float playerSpeed, gravity;

    private bool escaped = false;

    public float progress = 0;

    // Start is called before the first frame update
    void Start()
    {
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
        // Interact to open the cryo pod and get out; can only do this once (obviously)
        if (Input.GetKeyDown(KeyCode.E) && !escaped)
        {
            escaped = true;
            StartCoroutine(leavePodAnimSequence()); // animation sequence
        }
            
    }

    IEnumerator leavePodAnimSequence()
    {
        player.GetComponent<FirstPersonController>().enabled = false;
        GetComponentInChildren<PlayableDirector>().Play();
        yield return new WaitForSeconds(1);

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
    }
}
