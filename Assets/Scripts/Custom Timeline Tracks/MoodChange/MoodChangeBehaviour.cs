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
    public Color SkyboxTint;
    
    public Color DistanceFogColor;
    public float DistanceFogStart = 0;
    public float DistanceFogEnd = 3.7f;
    
    public Texture2D GradientFog;
    public float FogDensity;

    public override void OnPlayableCreate(Playable playable)
    {
    }


}
