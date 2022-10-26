using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLogController : MonoBehaviour
{
    private Light light;
    private float intensity;

    // Start is called before the first frame update
    void Start()
    {
        light = transform.GetChild(1).gameObject.GetComponent<Light>();
        intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        intensity += Time.deltaTime * 3;
        light.intensity = (Mathf.Sin(intensity) + 1) / 2.0f;
        Debug.Log(light.intensity);
    }
}
