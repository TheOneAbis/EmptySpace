using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLightController : MonoBehaviour
{
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray lookRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        // If player is looking at the light, press E to activate it
        if (GetComponent<SphereCollider>().bounds.IntersectRay(lookRay))
        {
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
