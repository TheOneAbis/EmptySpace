using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLogController : MonoBehaviour
{
    private Light logLight;
    private float intensity;
    public float blinkRate = 3;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            transform.GetChild(1);
            logLight = transform.GetChild(1).gameObject.GetComponent<Light>();
        }
        catch
        {
            logLight = GetComponent<Light>();
        }
        intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        intensity += Time.deltaTime * blinkRate;
        logLight.intensity = (Mathf.Sin(intensity) + 1) / 2.0f;
    }
}
