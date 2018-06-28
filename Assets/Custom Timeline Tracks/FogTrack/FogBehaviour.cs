using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class FogBehaviour : PlayableBehaviour
{
    public Color fogColor;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
