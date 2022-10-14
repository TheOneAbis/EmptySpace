using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject journal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Awake
    private void Awake()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Camera.main.transform.Rotate(0, 0.2f, 0);
    }

    public void CloseLogButton()
    {
        journal.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Playground");
    }

    public void StartButton()
    {
        canvas.SetActive(false);
        journal.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
