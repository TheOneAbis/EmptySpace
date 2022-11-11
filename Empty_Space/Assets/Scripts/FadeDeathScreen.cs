using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeDeathScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float elapsedTime;
    private float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
        elapsedTime = 0.0f;
        fadeTime = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(canvasGroup.alpha < 1.0f)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01((elapsedTime / fadeTime));
        }
        else
        {
            elapsedTime = 0.0f;
        }
    }
}