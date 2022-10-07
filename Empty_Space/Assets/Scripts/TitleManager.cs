using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class TitleManager : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake
    private void Awake()
    {
        //player.GetComponent<FirstPersonController>().enabled = false;
        //player.SetActive(false);
        //Cursor.visible = true;

        // Disabling the canvas for now, since it's supposed to be a separate scene anyway
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        player.GetComponent<FirstPersonController>().enabled = true;
        player.SetActive(true);
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
