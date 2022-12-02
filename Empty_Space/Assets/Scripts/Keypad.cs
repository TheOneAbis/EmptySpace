using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;
    private UIManagement UIManager;
    private float distanceSquared;
    private int number;
    private string correctSequence;
    private string inputSequence;
    private bool activated;

    public GameObject[] exitDoors;
    public float interactDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
        distanceSquared = (player.transform.position - transform.position).sqrMagnitude;

        correctSequence = "326";
        inputSequence = "000";
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            distanceSquared = (player.transform.position - transform.position).sqrMagnitude;

            // If player is within interaction distance of keypad
            if (distanceSquared < Mathf.Pow(interactDistance, 2))
            {
                // Find the button being looked at
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (IsLookedAt(transform.GetChild(i).gameObject))
                    {
                        number = i;
                        UIManager.DisplayCustomTooltip($"[Left Click] Input {i}");
                        break;
                    }
                    number = -1;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (number != -1)
                    {
                        inputSequence += $"{number}";
                        inputSequence = inputSequence.Substring(inputSequence.Length > 3 ? inputSequence.Length - 3 : 0);
                        Debug.Log(inputSequence);
                        StartCoroutine(ShowFeedback(transform.GetChild(number).gameObject));
                    }
                    if (inputSequence.Equals(correctSequence))
                    {
                        activated = true;
                        StartCoroutine(InvokeWinCondition());
                    }
                }
            }
        }
    }

    public bool IsLookedAt(GameObject button)
    {
        Ray lookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return button.GetComponent<BoxCollider>().bounds.IntersectRay(lookRay);
    }

    IEnumerator InvokeWinCondition()
    {
        player.GetComponent<FirstPersonController>().zeroG = false;
        player.GetComponent<FirstPersonController>().Gravity = -15.0f;
        enemy.GetComponent<MonsterChaseController>().mode = MonsterMode.None;
        enemy.GetComponent<MonsterChaseController>().canKill = false;

        // LET THERE BE LIGHT
        for (float i = 0; i <= 1.0f; i += Time.deltaTime * 3)
        {
            RenderSettings.ambientIntensity = Mathf.Lerp(1.0f, 2.25f, i);
            yield return new WaitForEndOfFrame();
        }
        enemy.transform.position = new Vector3(-53.5f, -5.0f, -66.0f); // move it out of view

        foreach (GameObject door in exitDoors)
            door.GetComponent<DoorController>().Unlock();

        yield return new WaitForSeconds(0.67f);

        enemy.GetComponent<MonsterChaseController>().SetSpeed(0.4f);
        enemy.GetComponent<MonsterChaseController>().SetGoal(player.transform.position);
        enemy.GetComponent<MonsterChaseController>().MoveToGoal();
        yield return new WaitForSeconds(4);

        // Stop emission but don't clear particles
        enemy.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        enemy.GetComponent<MonsterChaseController>().Stop();
        yield return new WaitForSeconds(5);
        enemy.transform.position += new Vector3(0, 500, 0); // move monster out of view
    }

    IEnumerator ShowFeedback(GameObject button)
    {
        Color temp = button.GetComponent<MeshRenderer>().material.color;
        button.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.cyan);

        yield return new WaitForSeconds(0.75f);
        button.GetComponent<MeshRenderer>().material.SetColor("_Color", temp);
    }
}
