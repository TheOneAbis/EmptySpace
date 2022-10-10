using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class LightControlBehavior : PlayableBehaviour
{
    [SerializeField]
    private Color color = new Color(1, 1, 1);

    [SerializeField]
    private float intensity = 1.0f;

    [SerializeField]
    private float bounceIntensity = 1.0f;

    [SerializeField]
    private float range = 10.0f;

    private Light light;

    private bool firstFrameHappened;
    private Color defaultColor;
    private float defaultIntensity;
    private float defaultBounceIntensity;
    private float defaultRange;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var light = playerData as Light;

        if (light == null) return;

        if (!firstFrameHappened)
        {
            defaultColor = light.color;
            defaultIntensity = light.intensity;
            defaultBounceIntensity = light.bounceIntensity;
            defaultRange = light.range;

            firstFrameHappened = true;
        }

        light.color = color;
        light.intensity = intensity;
        light.bounceIntensity = bounceIntensity;
        light.range = range;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        firstFrameHappened = false;

        if (light == null) return;

        light.color = defaultColor;
        light.intensity = defaultIntensity;
        light.bounceIntensity = defaultBounceIntensity;
        light.range = defaultRange;


        base.OnBehaviourPause(playable, info);
    }
}
