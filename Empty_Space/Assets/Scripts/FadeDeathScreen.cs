using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeDeathScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float elapsedTime;
    private float fadeTime;
    public bool fadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        elapsedTime = 0.0f;
        fadeTime = 4.0f;
        fadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canvasGroup.alpha > 0.0f && fadeOut)
        {
            canvasGroup.interactable = false;
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1.0f - (elapsedTime / fadeTime));
        }
        else if(elapsedTime > 4 || canvasGroup.alpha <= 0)
        {
            gameObject.SetActive(false);
            fadeOut = false;
            elapsedTime = 0.0f;
        }
    }
}
