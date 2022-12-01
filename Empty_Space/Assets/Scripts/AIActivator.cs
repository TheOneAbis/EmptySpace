using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIActivator : MonoBehaviour
{
    private GameObject player;
    private UIManagement UIManager;
    private RadialProgressController clickDisplayer;
    public GameObject consoleScreen;

    private float sqrDistance;
    public float keyHoldTime;
    private float keyHoldTimeLeft;
    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UIManager = GameObject.Find("UIManager").GetComponent<UIManagement>();
        clickDisplayer = GameObject.Find("RadialProgress").GetComponent<RadialProgressController>();
        keyHoldTimeLeft = keyHoldTime;
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {
            sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < 16)
            {
                UIManager.DisplayCustomTooltip("[Left Click] Activate Shipboard AI");
                if (Input.GetMouseButton(0))
                {
                    keyHoldTimeLeft -= Time.deltaTime;
                    clickDisplayer.clickDisplay(keyHoldTime - keyHoldTimeLeft, keyHoldTime);
                    if (keyHoldTimeLeft <= 0)
                    {
                        activated = true;
                        clickDisplayer.clickReset();

                        // Display that AI is activated and show interaction dialogue
                        transform.GetChild(0).GetComponent<TextMeshPro>().text = "Shipboard AI Active";
                        transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
                        consoleScreen.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Shipboard AI\n--Online--";
                        consoleScreen.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;

                        UIManager.QueueDialogue("Shipboard AI:", "Fetching AI Manifest... Complete. Detecting major power outage across all systems. " +
                            "Rerouting Auxiliary power... Complete. Shipboard AI is online.", 0, 8);
                        UIManager.QueueDialogue("Space Boi:", "Thank goodness the ship's AI still seems functional. Do you have a name?", 0.4f, 6);
                        UIManager.QueueDialogue("Shipboard AI:", "Detecting new cre member. Welcome! My designation is VIVY.", 0.4f, 6);
                        UIManager.QueueDialogue("Space Boi:", "Vivy, Huh. Interesting name... Right. Anyway, do you have any idea what " +
                            "happened to the ship? I can't seem to find anyone.", 0.25f, 6);
                        UIManager.QueueDialogue("VIVY:", "Running diagnostics check... The ship is currently in Emergency Critical-Power mode. Determining Cause... Unknmown.", 0.4f, 6.5f);
                        UIManager.QueueDialogue("VIVY:", "Scanning crew members... One crew member found currently aboard the ship.", 0.4f, 5);
                        UIManager.QueueDialogue("Space Boi:", "Just me?! That can't be right. I have to go check the living quarters. There's gotta be someone there.", 0.4f, 6.5f);
                    }
                }
            }
        }
    }
}
