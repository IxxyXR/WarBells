using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class MoodChangeBehaviour : PlayableBehaviour
{
    public Camera ControlledCamera;
    public Light ControlledLight;
    
    public Color LightColor;
    public Texture2D GradientFog;
    public Color DistanceFogColor;
    public float FogDensity;

    public override void OnPlayableCreate(Playable playable)
    {
    }


}
