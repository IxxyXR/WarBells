using System;
using UnityEditor.Recorder.Input;
using UnityEngine;

namespace UnityEditor.Recorder
{
    [Serializable]
    class ImageInputSelector : InputSettingsSelector
    {      
        [SerializeField] GameViewInputSettings m_GameViewInputSettings = new GameViewInputSettings();
        [SerializeField] CameraInputSettings m_CameraInputSettings = new CameraInputSettings();
        [SerializeField] Camera360InputSettings m_Camera360InputSettings = new Camera360InputSettings();
        [SerializeField] RenderTextureInputSettings m_RenderTextureInputSettings = new RenderTextureInputSettings();
        [SerializeField] RenderTextureSamplerSettings m_RenderTextureSamplerSettings = new RenderTextureSamplerSettings();

        public ImageInputSelector()
        {
            m_CameraInputSettings.forceEvenSize = true;
            m_GameViewInputSettings.forceEvenSize = true;
            m_RenderTextureInputSettings.renderTexture = null;
            m_Camera360InputSettings.flipFinalOutput = false;
            m_RenderTextureSamplerSettings.forceEvenSize = true;
        }
        
        public ImageInputSettings imageInputSettings
        {
            get { return (ImageInputSettings)selected; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value is CameraInputSettings ||
                    value is GameViewInputSettings ||
                    value is Camera360InputSettings ||
                    value is RenderTextureInputSettings ||
                    value is RenderTextureSamplerSettings)
                {
                    selected = value;
                }
                else
                {
                    throw new ArgumentException("Video input type not supported: '" + value.GetType() + "'");
                }
            }
        }
    }
    
    [Serializable]
    class UTJImageInputSelector : InputSettingsSelector
    {      
        [SerializeField] CameraInputSettings m_CameraInputSettings = new CameraInputSettings();
        [SerializeField] RenderTextureInputSettings m_RenderTextureInputSettings = new RenderTextureInputSettings();
        [SerializeField] RenderTextureSamplerSettings m_RenderTextureSamplerSettings = new RenderTextureSamplerSettings();

        public UTJImageInputSelector()
        {
            m_CameraInputSettings.forceEvenSize = true;
            m_RenderTextureInputSettings.renderTexture = null;
            m_RenderTextureSamplerSettings.forceEvenSize = true;
        }
        
        public ImageInputSettings imageInputSettings
        {
            get { return (ImageInputSettings)selected; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value is CameraInputSettings ||
                    value is RenderTextureInputSettings ||
                    value is RenderTextureSamplerSettings)
                {
                    selected = value;
                }
                else
                {
                    throw new ArgumentException("Video input type not supported: '" + value.GetType() + "'");
                }
            }
        }
        
        public void SetMaxResolution(ImageHeight maxSupportedSize)
        {           
            m_CameraInputSettings.maxSupportedSize = maxSupportedSize;
            m_RenderTextureSamplerSettings.maxSupportedSize = maxSupportedSize;
        }
    }
}