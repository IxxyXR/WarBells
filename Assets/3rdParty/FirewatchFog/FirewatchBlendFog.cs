using UnityEngine;


namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    public class FirewatchBlendFog : PostEffectsBase
    {

        public Texture2D mainTex;
        public float fogAmount = 1f;
        public Texture2D colorRamp1;
        public Texture2D colorRamp2;
        public float blendAmount = 0.5f;
        public float fogIntensity = 1f;
        
        private Shader fogShader = null;
        private Material fogMaterial = null;

        public override bool CheckResources ()
        {
            CheckSupport (true);
            fogShader = Shader.Find("Custom/FirewatchBlendFog");
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
            fogMaterial.SetTexture("_ColorRamp1", colorRamp1);
            fogMaterial.SetTexture("_ColorRamp2", colorRamp2);
            fogMaterial.SetFloat("_BlendAmount", blendAmount);
            fogMaterial.SetFloat("_FogIntensity", fogIntensity);
            Graphics.Blit(source, destination, fogMaterial);
        }
    }
}