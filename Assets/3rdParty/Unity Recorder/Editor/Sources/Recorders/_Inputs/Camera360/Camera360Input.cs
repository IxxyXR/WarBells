using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor.Recorder.Input
{
    class Camera360Input : BaseRenderTextureInput
    {
        bool m_ModifiedResolution;
        Shader m_shCopy;
        TextureFlipper m_VFlipper = new TextureFlipper();

        RenderTexture m_Cubemap1;
        RenderTexture m_Cubemap2;

        public Camera360InputSettings settings360
        {
            get { return (Camera360InputSettings)settings; }
        }

        Camera targetCamera { get; set; }

        public Shader copyShader
        {
            get
            {
                if (m_shCopy == null)
                {
                    m_shCopy = Shader.Find("Hidden/Recorder/Inputs/CBRenderTexture/CopyFB");
                }
                return m_shCopy;
            }

            set { m_shCopy = value; }
        }

        public override void BeginRecording(RecordingSession session)
        {
            if (settings360.flipFinalOutput)
                m_VFlipper = new TextureFlipper();
            outputWidth = settings360.outputWidth;
            outputHeight = settings360.outputHeight;
        }

        public override void NewFrameStarting(RecordingSession session)
        {
            switch (settings360.source)
            {
                case ImageSource.MainCamera:
                {
                    if (targetCamera != Camera.main )
                        targetCamera = Camera.main;
                    break;
                }

                case ImageSource.TaggedCamera:
                {
                    var tag = settings360.cameraTag;

                    if (targetCamera == null || !targetCamera.gameObject.CompareTag(tag) )
                    {
                        try
                        {
                            var cams = GameObject.FindGameObjectsWithTag(tag);
                            if (cams.Length > 0)
                                Debug.LogWarning("More than one camera has the requested target tag:" + tag);
                            targetCamera = cams[0].transform.GetComponent<Camera>();
                            
                        }
                        catch (UnityException)
                        {
                            Debug.LogWarning("No camera has the requested target tag:" + tag);
                            targetCamera = null;
                        }
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PrepFrameRenderTexture();

        }

        public override void NewFrameReady(RecordingSession session)
        {
            var eyesEyeSepBackup = targetCamera.stereoSeparation;
            var eyeMaskBackup = targetCamera.stereoTargetEye;
            if (settings360.renderStereo)
            {
                targetCamera.stereoSeparation = settings360.stereoSeparation;
                targetCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                targetCamera.RenderToCubemap(m_Cubemap1, 63, Camera.MonoOrStereoscopicEye.Left);
                targetCamera.stereoSeparation = settings360.stereoSeparation;
                targetCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                targetCamera.RenderToCubemap(m_Cubemap2, 63, Camera.MonoOrStereoscopicEye.Right);
            }
            else
            {
                targetCamera.RenderToCubemap(m_Cubemap1, 63, Camera.MonoOrStereoscopicEye.Mono);
            }
            
            if (settings360.renderStereo)
            {
                m_Cubemap1.ConvertToEquirect(outputRT, Camera.MonoOrStereoscopicEye.Left);
                m_Cubemap2.ConvertToEquirect(outputRT, Camera.MonoOrStereoscopicEye.Right);
            }
            else
            {
                m_Cubemap1.ConvertToEquirect(outputRT, Camera.MonoOrStereoscopicEye.Mono);
            }
            
            if (settings360.flipFinalOutput)
                m_VFlipper.Flip(outputRT);
                
            targetCamera.stereoSeparation = eyesEyeSepBackup;
            targetCamera.stereoTargetEye = eyeMaskBackup;
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if( m_Cubemap1 )
                    UnityHelpers.Destroy(m_Cubemap1);
                if( m_Cubemap2 )
                    UnityHelpers.Destroy(m_Cubemap2);

                if( m_VFlipper!=null )
                    m_VFlipper.Dispose();
            }

            base.Dispose(disposing);
        }

        void PrepFrameRenderTexture()
        {
            if (outputRT != null)
            {
                if (outputRT.IsCreated() && outputRT.width == outputWidth && outputRT.height == outputHeight)
                {
                    return;
                }

                ReleaseBuffer();
            }

            outputRT = new RenderTexture(outputWidth, outputHeight, 24, RenderTextureFormat.ARGB32)
            {
                dimension = TextureDimension.Tex2D,
                antiAliasing = 1
            };
            m_Cubemap1 = new RenderTexture(settings360.mapSize, settings360.mapSize, 24, RenderTextureFormat.ARGB32)
            {
                dimension = TextureDimension.Cube ,
                
            };
            m_Cubemap2 = new RenderTexture(settings360.mapSize, settings360.mapSize, 24, RenderTextureFormat.ARGB32)
            {
                dimension = TextureDimension.Cube 
            };
        }
    }
}