using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class LightControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField]
    private LightControlBehavior template = new LightControlBehavior();

    // ClipCaps specify properties of the clip
    public ClipCaps clipCaps { get { return ClipCaps.None; } }

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<LightControlBehavior>.Create(graph, template);
    }
}
