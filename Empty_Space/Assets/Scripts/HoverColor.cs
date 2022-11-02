using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverColor : MonoBehaviour
{
    public Image myImage;
    public Color wantedColor;
    public Color originalColor;

    public void changeWhenHover()
    {
        myImage.color = wantedColor;
    }

    public void changeWhenLeaves()
    {
        myImage.color = originalColor;
    }
}
