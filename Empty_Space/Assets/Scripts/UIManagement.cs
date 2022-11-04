using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tooltip
{
    Interact,
    Move,
    Sprint,
    Look,
    LeftClick,
    None
};
public class UIManagement : MonoBehaviour
{
    public bool eUI = true;
    public bool wASDUI = false;
    public bool sprintUI = false;
    public bool delay = false;
    public bool mouseUI = true;
    private float timer = 0.0f;
    private float goal;

    public GameObject[] HUDCanvases;
    private Tooltip currentTooltip;
    private float tooltipTimer;

    // Start is called before the first frame update
    void Start()
    {
        goal = 10.0f;
        tooltipTimer = 0.0f;
        DisplayTooltip(Tooltip.Look);
        foreach (GameObject canvas in HUDCanvases)
            canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (GameObject canvas in HUDCanvases)
        //    canvas.SetActive(false);
        HUDCanvases[4].SetActive(false);
        HUDCanvases[0].SetActive(false);

        if (currentTooltip != Tooltip.None)
            HUDCanvases[(int)currentTooltip].SetActive(true);

        // Keep tooltip displayed until timer hits 0
        if (tooltipTimer > 0.0f) 
            tooltipTimer -= Time.deltaTime;
        else
            currentTooltip = Tooltip.None;

        if (!mouseUI)
            HUDCanvases[3].SetActive(false);

        if (wASDUI)
        {
            timer += Time.deltaTime;
            if (timer >= goal)
            {
                timer = 0.0f;
                wASDUI = false;
                sprintUI = true;
                HUDCanvases[1].SetActive(false);
                HUDCanvases[2].SetActive(true);
            }
        }
        else if (sprintUI)
        {
            timer += Time.deltaTime;
            if (timer >= goal)
            {
                timer = 0.0f;
                sprintUI = false;
                HUDCanvases[2].SetActive(false);
            }
        }
        else if(delay)
        {
            timer += Time.deltaTime;
            if(timer >= 4.8f)
            {
                wASDUI = true;
                HUDCanvases[1].SetActive(true);
                delay = false;
            }
        }


    }
   
    // Display a tooltip on the screen (to be used in update methods, as this appears for one frame)
    public void DisplayTooltip(Tooltip tooltip)
    {
        currentTooltip = tooltip;
        tooltipTimer = 0.0f;
    }

    // Display a tooltip on the screen for a specified amount of seconds
    public void DisplayTooltip(Tooltip tooltip, float seconds)
    {
        currentTooltip = tooltip;
        tooltipTimer = seconds;
    }
}
