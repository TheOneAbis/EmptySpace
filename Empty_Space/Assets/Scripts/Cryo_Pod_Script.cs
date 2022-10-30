using Cinemachine;
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
    private CinemachineVirtualCamera mainCam;
    private float playerSpeed, gravity;
    private GameObject[] switches;
    private bool canExit;
    private GameObject UIManager;

    private bool escaped = false;
    private float camStartFOV;
    private float clickForce; // max amt to force open the pod

    public float progress = 0;

    // Start is called before the first frame update
    void Start()
    {
        UIManager = GameObject.Find("UIManager");

        switches = GameObject.FindGameObjectsWithTag("ExitLight");
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();

        // temporarily store these value for re-enabling movement
        playerSpeed = player.GetComponent<FirstPersonController>().MoveSpeed;
        gravity = player.GetComponent<FirstPersonController>().Gravity;

        // disable player physics, essentially
        player.GetComponent<FirstPersonController>().MoveSpeed = 0;
        player.GetComponent<FirstPersonController>().Gravity = 0;

        camStartFOV = mainCam.m_Lens.FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        // Player can exit if all lights are activated
        canExit = true;
        foreach (GameObject s in switches)
            if (!s.GetComponent<ExitLightController>().Activated) 
                canExit = false;

        // After pressing the buttons, force your way out by spamming left click
        if (canExit)
        {
            UIManager.GetComponent<UIManagement>().DisplayTooltip(Tooltip.LeftClick);
            mainCam.m_Lens.FieldOfView = camStartFOV - clickForce; // alter fov base on click amt
            if (Input.GetMouseButton(0) && !escaped) clickForce += Time.deltaTime * 8;
            else if (clickForce > 0) // slowly decrease force - player must hold down
                clickForce -= (Time.deltaTime * 3);
        }

        // If enough force applied (clicked enough), Begin cyo pod escape cutscene
        if (clickForce > 10 && !escaped)
        {
            UIManager.GetComponent<UIManagement>().delay = true;
            UIManager.GetComponent<UIManagement>().DisplayTooltip(Tooltip.None);
            escaped = true;
            foreach (GameObject s in switches) s.SetActive(false);
            StartCoroutine(leavePodAnimSequence()); // animation sequence
        }


        // -- DEV CHEAT - SKIP THIS MECHANIC -- \\
        if (Input.GetKeyDown(KeyCode.Alpha1) && !escaped)
            DevSkip();
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
        enabled = false; // disable this script
    }

    private void DevSkip()
    {
        escaped = true;
        // TP player to DevStart, if it exists
        if (GameObject.Find("DevStart") != null)
        {
            Debug.Log("asd");
            player.transform.position = GameObject.Find("DevStart").transform.position;
        }
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
        player.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        foreach (GameObject s in switches) s.SetActive(false);
        player.GetComponent<FirstPersonController>().MoveSpeed = playerSpeed;
        player.GetComponent<FirstPersonController>().Gravity = gravity;
        player.GetComponent<FirstPersonController>().enabled = true;
        player.GetComponent<FirstPersonController>().UpdateCinemachineTargetPitch();
        player.GetComponent<AudioSource>().Play(); // begin ambient music loop

        enabled = false;
    }
}
