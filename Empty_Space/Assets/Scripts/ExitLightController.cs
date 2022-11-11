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
        // If player is looking at the light, press E to activate it
        if (IsLookedAt())
        {
            UIManager.GetComponent<UIManagement>().mouseUI = false;
            if (Input.GetKey(KeyCode.E))
            {
                Activated = true;
                GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);
                GetComponent<Light>().color = Color.green;
            }
        }
    }

    public bool IsLookedAt()
    {
        Ray lookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return GetComponent<SphereCollider>().bounds.IntersectRay(lookRay);
    }

    public bool Activated
    {
        get { return active; }
        set { active = value; }
    }
}
