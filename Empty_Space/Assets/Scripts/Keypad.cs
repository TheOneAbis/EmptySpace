using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    private GameObject player;
    private GameObject enemy;
    private UIManagement UIManager;
    private float distanceSquared;
    private float number;
    private string correctSequence;
    private string inputSequence;

    public GameObject exitDoor;
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
    }

    // Update is called once per frame
    void Update()
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
                    inputSequence.Remove(0, 1);
                }
                if (inputSequence.Equals(correctSequence))
                    StartCoroutine(InvokeWinCondition());
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
        enemy.transform.position = enemy.GetComponent<MonsterChaseController>().engineRoomSpawn; // move it out of view
        yield return new WaitForSeconds(1);
        enemy.GetComponent<MonsterChaseController>().SetSpeed(3.0f);
        enemy.GetComponent<MonsterChaseController>().SetGoal(player.transform.position);
        enemy.GetComponent<MonsterChaseController>().MoveToGoal();
        yield return new WaitForSeconds(4);

        // Stop emission but don't clear particles
        enemy.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        enemy.GetComponent<MonsterChaseController>().Stop();
        yield return new WaitForSeconds(5);
        enemy.transform.position += new Vector3(0, 500, 0); // move monster out of view
    }
}
