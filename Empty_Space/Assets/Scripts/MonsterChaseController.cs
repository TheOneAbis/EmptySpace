using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum MonsterMode
{
    Hallway,
    Patrol,
    Chase,
    None
}

public class MonsterChaseController : MonoBehaviour
{
    

    private Vector3 goal;
    private bool shouldMove;
    private GameObject player;
    private CheckpointController cpController;
    private CinemachineVirtualCamera mainCam;
    private AudioController gameAudio;

    public float hallMoveSpeed;
    public float patrolMoveSpeed;
    public float triggeredMoveSpeed;
    private float customMoveSpeed;
    public bool canKill;
    public GameObject deathCanvas;
    public GameObject puzzle4;
    public AudioClip SuccessSound;

    public GameObject mainCPs;

    public MonsterMode mode;
    private Vector3 engineRoomSpawn;


    // Start is called before the first frame update
    void Start()
    {
        customMoveSpeed = 0.0f;
        canKill = false;
        goal = transform.position;
        shouldMove = false;
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cpController = GameObject.Find("CheckpointManager").GetComponent<CheckpointController>();
        gameAudio = GameObject.Find("AudioPlayer").GetComponent<AudioController>();
        mode = MonsterMode.Hallway;
        engineRoomSpawn = new Vector3(-62.5f, -2.5f, -54.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the monster if it is set to move toward the goal
        if (shouldMove)
        {
            if ((goal - transform.position).sqrMagnitude > Mathf.Pow(0.1f, 2))
            {
                float speed;
                if (mode == MonsterMode.Hallway) speed = hallMoveSpeed;
                else if (mode == MonsterMode.Patrol) speed = patrolMoveSpeed;
                else if (mode == MonsterMode.Chase) speed = 7.0f;
                else speed = customMoveSpeed;

                //GetComponent<Rigidbody>().AddForce((goal - transform.position).normalized * moveSpeed * Time.deltaTime);
                transform.position += (goal - transform.position).normalized * speed * Time.deltaTime;
            }
            else
                shouldMove = false;
        }

        // patrol mode functionality
        if (mode == MonsterMode.Patrol)
        {
            if (!shouldMove)
            {
                SetGoal(FindNextCPPos());
                MoveToGoal();
            }

            if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 45.0f && 
                (player.transform.position - transform.position).sqrMagnitude < Mathf.Pow(30, 2))
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, player.transform.position - transform.position, out hit);
                if (hit.collider == player.GetComponentInChildren<CapsuleCollider>())
                {
                    Debug.Log("AAAHAHAHAH");
                    mode = MonsterMode.Chase;
                    SetGoal(player.transform.position + ((player.transform.position - transform.position).normalized * 1000)); // keep going past the player
                    MoveToGoal();
                }
            }
        }
    }

    public void SetGoal(Vector3 goalPosition)
    {
        StartCoroutine(Turn(goalPosition));
        goal = goalPosition;
    }

    public void MoveToGoal()
    {
        shouldMove = true;
    }

    /// <summary>
    /// Sets the speed of the monster when it is in MonsterMode.None
    /// </summary>
    /// <param name="speed">The speed to set it to</param>
    public void SetSpeed(float speed)
    {
        customMoveSpeed = speed;
    }

    public void Stop()
    {
        if (shouldMove)
            shouldMove = false;
    }

    public void InitPatrolMode()
    {
        // Reduce the X bounds of the BV to better fit the monster now that we are in an open area, not the corridor
        GetComponent<BoxCollider>().size = new Vector3(4, 4, 4);
        // TP monster to engine room and have it immediately look at the player
        transform.position = engineRoomSpawn;
        transform.LookAt(player.transform.position);
        mode = MonsterMode.Patrol;
    }

    public Vector3 FindNextCPPos()
    {
        List<GameObject> visibleCPs = new List<GameObject>();
        RaycastHit hit;

        for (int i = 0; i < mainCPs.transform.childCount; i++)
        {
            Physics.Raycast(transform.position, mainCPs.transform.GetChild(i).position - transform.position, out hit);
            if (hit.collider == mainCPs.transform.GetChild(i).GetComponent<SphereCollider>())
                visibleCPs.Add(mainCPs.transform.GetChild(i).gameObject);
        }

        return visibleCPs[Random.Range(0, visibleCPs.Count)].transform.position;
    }

    public void ResetWithRandomCP()
    {
        if (mode != MonsterMode.None)
        {
            mode = MonsterMode.Patrol;
            transform.position = mainCPs.transform.GetChild(Random.Range(0, mainCPs.transform.childCount)).transform.position;
            SetGoal(FindNextCPPos());
            MoveToGoal();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mode == MonsterMode.Hallway)
        {
            if (other.gameObject == player)
            {
                if (canKill) StartCoroutine(DeathSequence());
            }
            else if (other.gameObject.name == "JammedDoor3")
            {
                if (other.GetComponent<DoorController>().locked == true)
                {
                    Stop();
                    StartCoroutine(EscapeSequence());
                }
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                if (canKill)
                {
                    StartCoroutine(DeathSequence());
                    Debug.Log("Player dead. Resetting.");
                }
               
            }
        }
    }

    IEnumerator DeathSequence()
    {
        Stop();
        //check if canvas is open and close it
        if(GameObject.Find("InteractionManager").GetComponent<InteractScript>().inUI)
        {
            puzzle4.SetActive(false);
        }
        player.GetComponent<FirstPersonController>().enabled = false;
        float originalFOV = mainCam.m_Lens.FieldOfView;

        Quaternion startRotation = player.transform.rotation;
        Vector3 lookDirection = (transform.position - player.transform.position);
        lookDirection.y = 0;

        Quaternion camStart = player.transform.GetChild(0).localRotation;

        for (float i = 0; i < 1.0f; i += Time.deltaTime * 4)
        {
            player.transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(lookDirection.normalized, player.transform.up), i);
            player.transform.GetChild(0).localRotation = Quaternion.Lerp(camStart, Quaternion.Euler(0, 0, 0), i); // set look elevation to 0
            yield return new WaitForSeconds(Time.deltaTime);
        }

        for (float i = 0; i < 1.0f; i += Time.deltaTime * 4)
        {
            mainCam.m_Lens.FieldOfView -= 125 * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(0.5f);

        player.GetComponent<FirstPersonController>().UpdateCinemachineTargetPitch();
        // TODO: Invoke death screen here
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        deathCanvas.GetComponent<CanvasGroup>().alpha = 1.0f;
        deathCanvas.GetComponent<CanvasGroup>().interactable = true;

        gameAudio.StopPlaying(); // Stop playing scary music
        deathCanvas.SetActive(true);

        cpController.Respawn();

        mainCam.m_Lens.FieldOfView = originalFOV; // reset fov
    }

    IEnumerator EscapeSequence()
    {
        gameAudio.SwitchAudio(SuccessSound, .025f, false);
        yield return new WaitForSeconds(3.0f);
        GetComponent<AudioSource>().Stop();
        transform.position += new Vector3(0, 100, 0);

        for (float i = 0; i < 1.0f; i += Time.deltaTime / 10)
        {
            RenderSettings.ambientIntensity = i;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Turn(Vector3 goalPosition)
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.LookRotation(goalPosition - transform.position);

        for (float i = 0; i < 1.0f; i += Time.deltaTime * 5)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
