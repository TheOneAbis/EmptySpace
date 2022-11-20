using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using TMPro;

public enum Tooltip
{
    Interact,
    Move,
    Sprint,
    Look,
    Custom,
    None
};
public class UIManagement : MonoBehaviour
{
    public bool eUI = true;
    public bool wASDUI = false;
    public bool sprintUI = false;
    public bool delay = false;
    private float timer = 0.0f;
    private float goal;
    private float rotationSpeed;
    private float speed;
    public bool paused = false;

    public GameObject[] HUDCanvases;
    private Tooltip currentTooltip;
    private float tooltipTimer;

    public GameObject interactionManager;
    public Slider pauseSlider;
    public GameObject player;

    public GameObject pauseCanvas;

    public GameObject deathCanvas;
    public GameObject deathText;
    public GameObject deathRetry;
    public GameObject deathQuit;

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
        if (Input.GetKeyDown(KeyCode.Escape) && !interactionManager.GetComponent<InteractScript>().inUI && !paused)
        {
            rotationSpeed = player.GetComponent<FirstPersonController>().RotationSpeed;
            player.GetComponent<FirstPersonController>().RotationSpeed = 0;
            speed = player.GetComponent<FirstPersonController>().MoveSpeed;
            player.GetComponent<FirstPersonController>().MoveSpeed = 0;
            pauseCanvas.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            paused = true;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            pauseCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player.GetComponent<FirstPersonController>().RotationSpeed = rotationSpeed;
            player.GetComponent<FirstPersonController>().MoveSpeed = speed;
            paused = false;
            Time.timeScale = 1;
        }

        foreach (GameObject canvas in HUDCanvases)
            canvas.SetActive(false);

        if (currentTooltip != Tooltip.None)
            HUDCanvases[(int)currentTooltip].SetActive(true);

        // Keep tooltip displayed until timer hits 0
        
        if (tooltipTimer > 0.0f)
            tooltipTimer -= Time.deltaTime;
        else
            currentTooltip = Tooltip.None;

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

    public void DisplayCustomTooltip(string message)
    {
        currentTooltip = Tooltip.Custom;
        HUDCanvases[(int)currentTooltip].GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    // Display a tooltip on the screen for a specified amount of seconds
    public void DisplayTooltip(Tooltip tooltip, float seconds)
    {
        currentTooltip = tooltip;
        tooltipTimer = seconds;
    }

    public void ContinueButton()
    {
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<FirstPersonController>().RotationSpeed = rotationSpeed;
        player.GetComponent<FirstPersonController>().MoveSpeed = speed;
        paused = false;
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void Slider()
    {
        rotationSpeed = pauseSlider.value;
    }

    public void RetryButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathCanvas.GetComponent<FadeDeathScreen>().fadeOut = true;
        deathText.SetActive(false);
        deathRetry.SetActive(false);
        deathQuit.SetActive(false);

        // Transfrom the player back to the checkpoint position here:
        GameObject.Find("CheckpointManager").GetComponent<CheckpointController>().Respawn();
    }
}
