using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgressController : MonoBehaviour
{
    private float clicks;
    private float requiredClicks;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        gameObject.GetComponent<Image>().fillAmount = 0;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            gameObject.GetComponent<Image>().fillAmount = clicks / requiredClicks;
        }
    }

    public void clickDisplay(float clickNum, float clickRequirement)
    {
        gameObject.GetComponent<Image>().fillAmount = clickNum / clickRequirement;
    }

    public void clickReset()
    {
        gameObject.GetComponent<Image>().fillAmount = 0;
    }
}
