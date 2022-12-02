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

// Struct to hold dialogue information
public struct UIDialogue
{
    public readonly string name;
    public readonly string message;
    public readonly float time;
    public readonly float delay;
    public UIDialogue(string name, string message, float time, float delay)
    {
        this.name = name;
        this.message = message;
        this.time = time;
        this.delay = delay;
    }
}

public class UIManagement : MonoBehaviour
{
    public bool eUI = true;
    public bool wASDUI = false;
    public bool sprintUI = false;
    public bool delay = false;
    private float rotationSpeed;
    private float speed;
    public bool paused = false;

    public GameObject[] HUDCanvases;
    private Tooltip currentTooltip;

    private float tooltipTimer;
    private float dialogueTimer;
    private float dialogueDelay;

    public GameObject interactionManager;
    public Slider pauseSlider;
    public GameObject player;

    public GameObject pauseCanvas;

    public GameObject deathCanvas;
    public GameObject deathText;
    public GameObject deathRetry;
    public GameObject deathQuit;

    public GameObject dialogueCanvas;

    private Queue<UIDialogue> dialogueQueue;

    private void Awake()
    {
        dialogueQueue = new Queue<UIDialogue>();
    }

    // Start is called before the first frame update
    void Start()
    {
        tooltipTimer = 0.0f;
        dialogueTimer = 0.0f;
        dialogueDelay = 0.0f;
        DisplayTooltip(Tooltip.Look);
        foreach (GameObject canvas in HUDCanvases)
            canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Press ESC to pause/unpause the game
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

        // Set all HUD canvases to inactive, and reactivate one depending on tooltip state per frame
        foreach (GameObject canvas in HUDCanvases)
            canvas.SetActive(false);

        if (currentTooltip != Tooltip.None)
            HUDCanvases[(int)currentTooltip].SetActive(true);

        // If tooltip isn't displayed indefinitely, keep tooltip displayed until timer hits 0
        tooltipTimer -= Time.deltaTime;
        if (tooltipTimer < 0.0f)
            currentTooltip = Tooltip.None;

        // Display dialogue in the dialogue queue
        dialogueDelay -= Time.deltaTime;
        if (dialogueDelay < 0)
        {
            dialogueTimer -= Time.deltaTime;
            dialogueCanvas.SetActive(true);
        }
        if (dialogueTimer < 0)
        {
            try
            {
                UIDialogue currentDialogue = dialogueQueue.Dequeue();
                TextMeshProUGUI[] canvasElements = dialogueCanvas.GetComponentsInChildren<TextMeshProUGUI>();
                canvasElements[0].text = currentDialogue.name;
                canvasElements[1].text = currentDialogue.message;
                dialogueTimer = currentDialogue.time;
                dialogueDelay = currentDialogue.delay;
            }
            catch {}

            dialogueCanvas.SetActive(false);
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

    public void DisplayCustomTooltip(string message, float seconds)
    {
        currentTooltip = Tooltip.Custom;
        HUDCanvases[(int)currentTooltip].GetComponentInChildren<TextMeshProUGUI>().text = message;
        tooltipTimer = seconds;
    }

    // Display a tooltip on the screen for a specified amount of seconds
    public void DisplayTooltip(Tooltip tooltip, float seconds)
    {
        currentTooltip = tooltip;
        tooltipTimer = seconds;
    }

    /// <summary>
    /// Queues a new dialogue quote to display on the player's screen.
    /// </summary>
    /// <param name="name">The name of the person saying the dialogue.</param>
    /// <param name="quote">The message the person is saying.</param>
    /// <param name="delay">Amount of seconds to wait before displaying this dialogue.</param>
    /// <param name="seconds">Amount of seconds this dialogue should be displayed for.</param>
    public void QueueDialogue(string name, string quote, float delay, float seconds)
    {
        dialogueQueue.Enqueue(new UIDialogue(name, quote, seconds, delay));
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
