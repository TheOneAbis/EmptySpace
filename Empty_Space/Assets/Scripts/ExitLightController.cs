using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLightController : MonoBehaviour
{
    private bool active = false;
    private GameObject UIManager;

    // Start is called before the first frame update
    void Start()
    {
        UIManager = GameObject.Find("UIManager");
    }

    // Update is called once per frame
    void Update()
    {
        Ray lookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        // If player is looking at the light, press E to activate it
        if (GetComponent<SphereCollider>().bounds.IntersectRay(lookRay))
        {
            UIManager.GetComponent<UIManagement>().mouseUI = false;
            if (!active) UIManager.GetComponent<UIManagement>().DisplayTooltip(Tooltip.Interact);
            if (Input.GetKey(KeyCode.E))
            {
                active = true;
                GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
                GetComponent<Light>().color = Color.green;
            }
        }
    }

    public bool Activated
    {
        get { return active; }
        set { active = value; }
    }
}
