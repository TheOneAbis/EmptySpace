using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagement : MonoBehaviour
{
    public bool eUI = true;
    public bool wASDUI = false;
    public bool sprintUI = false;
    public bool delay = true;
    private float timer = 0.0f;
    private float goal;
    public GameObject eCanvas;
    public GameObject wasdCanvas;
    public GameObject sprintCanvas;

    // Start is called before the first frame update
    void Start()
    {
        goal = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (eUI)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                timer = 0.0f;
                eUI = false;
                eCanvas.SetActive(false);
            }
        }
        else if (wASDUI)
        {
            timer += Time.deltaTime;
            if (timer >= goal)
            {
                timer = 0.0f;
                wASDUI = false;
                sprintUI = true;
                wasdCanvas.SetActive(false);
                sprintCanvas.SetActive(true);
                delay = false;
            }
        }
        else if (sprintUI)
        {
            timer += Time.deltaTime;
            if (timer >= goal)
            {
                timer = 0.0f;
                sprintUI = false;
                sprintCanvas.SetActive(false);
            }
        }
        else if(delay)
        {
            timer += Time.deltaTime;
            if(timer >= 2.5f)
            {
                wASDUI = true;
                wasdCanvas.SetActive(true);
            }
        }
        Debug.Log(timer);
    }
}
