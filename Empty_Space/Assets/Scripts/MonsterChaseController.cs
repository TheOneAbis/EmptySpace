using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using StarterAssets;

public class MonsterChaseController : MonoBehaviour
{
    private Vector3 goal;
    public float moveSpeed;
    private bool shouldMove;
    public bool canKill;
    private GameObject player;
    private CheckpointController cpController;
    private CinemachineVirtualCamera mainCam;
    public GameObject deathCanvas;

    // Start is called before the first frame update
    void Start()
    {
        canKill = false;
        goal = transform.position;
        shouldMove = false;
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        cpController = GameObject.Find("CheckpointManager").GetComponent<CheckpointController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the monster if it is set to move toward the goal
        if (shouldMove)
        {
            if ((goal - transform.position).magnitude > 0.1f)
                //GetComponent<Rigidbody>().AddForce((goal - transform.position).normalized * moveSpeed * Time.deltaTime);
                transform.position += (goal - transform.position).normalized * moveSpeed * Time.deltaTime;
            else
                shouldMove = false;
        }
    }

    public void SetGoal(Vector3 goalPosition)
    {
        goal = goalPosition;
    }

    public void MoveToGoal()
    {
        shouldMove = true;
    }

    public void Stop()
    {
        if (shouldMove)
            shouldMove = false;
    }

    private void OnTriggerEnter(Collider other)
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

    IEnumerator DeathSequence()
    {
        Stop();
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
        deathCanvas.GetComponent<CanvasGroup>().alpha = 0.0f;
        deathCanvas.SetActive(true);

        cpController.Respawn();

        mainCam.m_Lens.FieldOfView = originalFOV; // reset fov
    }

    IEnumerator EscapeSequence()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<AudioSource>().Stop();
        transform.position += new Vector3(0, 100, 0);

        for (float i = 0; i < 1.0f; i += Time.deltaTime / 10)
        {
            RenderSettings.ambientIntensity = i;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
