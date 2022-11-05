using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

public class MonsterChaseController : MonoBehaviour
{
    private Vector3 goal;
    public float moveSpeed;
    private bool shouldMove;
    private GameObject player;
    private CinemachineVirtualCamera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        goal = transform.position;
        shouldMove = false;
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the monster if it is set to move toward the goal
        if (shouldMove)
        {
            if ((goal - transform.position).magnitude > 0.1f)
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
            StartCoroutine(DeathSequence());
        }
    }

    IEnumerator DeathSequence()
    {
        Stop();
        player.GetComponent<FirstPersonController>().enabled = false;

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

        // TODO: Invoke death screen here
    }
}
