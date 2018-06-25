using UnityEngine;


namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    public class FirewatchFog : PostEffectsBase
    {

        public Texture2D mainTex;
        public float fogAmount;
        public Texture2D colorRamp;
        public float fogIntensity;
        
        public Shader fogShader = null;
        private Material fogMaterial = null;

        public override bool CheckResources ()
        {
            CheckSupport (true);			
            fogMaterial = CheckShaderAndCreateMaterial (fogShader, fogMaterial);
            if (!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        [ImageEffectOpaque]
        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            if (CheckResources()==false)
            {
                Graphics.Blit (source, destination);
                return;
            }

            fogMaterial.SetTexture("_MainTex", mainTex);
            fogMaterial.SetFloat("_FogAmount", fogAmount);
            fogMaterial.SetTexture("_ColorRamp", colorRamp);
            fogMaterial.SetFloat("_FogIntensity", fogIntensity);
            Graphics.Blit(source, destination, fogMaterial);
        }
    }
}